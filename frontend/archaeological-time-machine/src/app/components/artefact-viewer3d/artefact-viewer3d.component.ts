import {
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
  ViewChild,
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import * as THREE from 'three';
import { Artefact } from '../../models/archaeology.models';

@Component({
  selector: 'app-artefact-viewer3d',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="artefact-viewer-container">
      <div class="viewer-header">
        <div class="header-info">
          <span class="badge-3d">3D WebGL Gallery Lab</span>
          <h4 class="artefact-title">{{ artefact?.name || 'Diagnostic Artefact 3D Inspection' }}</h4>
          <p class="artefact-meta" *ngIf="artefact">
            <span class="meta-tag">{{ artefact.material }}</span>
            <span class="meta-tag" *ngIf="artefact.approximateYearFormatted">{{ artefact.approximateYearFormatted }}</span>
            <span class="meta-tag">{{ artefact.currentLocation }}</span>
          </p>
        </div>
        <div class="viewer-controls">
          <button
            class="control-btn"
            [class.active]="isAutoRotating()"
            (click)="toggleAutoRotate()"
            title="Toggle Continuous Rotation"
          >
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M21.5 2v6h-6M21.34 15.57a10 10 0 1 1-.57-8.38l5.67-5.67"/>
            </svg>
            {{ isAutoRotating() ? 'Spinning' : 'Static' }}
          </button>
          <button class="control-btn" (click)="resetView()" title="Reset Camera View">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M3 12a9 9 0 1 0 9-9 9.75 9.75 0 0 0-6.74 2.74L3 8"/>
              <path d="M3 3v5h5"/>
            </svg>
            Reset
          </button>
        </div>
      </div>

      <div class="canvas-wrapper" #canvasContainer (mousedown)="onMouseDown($event)" (mousemove)="onMouseMove($event)" (mouseup)="onMouseUp()" (wheel)="onWheel($event)">
        <div class="canvas-hint">Drag to rotate • Scroll to zoom</div>
      </div>

      <div class="artefact-discovery-note" *ngIf="artefact?.discoveryContext">
        <strong>Discovery Horizon:</strong> {{ artefact?.discoveryContext }}
      </div>
    </div>
  `,
  styles: [`
    .artefact-viewer-container {
      display: flex;
      flex-direction: column;
      background: #ffffff;
      border: 1px solid rgba(0, 0, 0, 0.1);
      border-radius: 14px;
      overflow: hidden;
      box-shadow: 0 8px 30px rgba(0, 0, 0, 0.08);
    }

    .viewer-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      padding: 16px 20px;
      background: #fafafa;
      border-bottom: 1px solid rgba(0, 0, 0, 0.06);
    }

    .badge-3d {
      display: inline-block;
      font-size: 10px;
      font-weight: 700;
      letter-spacing: 0.08em;
      text-transform: uppercase;
      color: #92400e;
      background: #fef8e7;
      border: 1px solid #fef3c7;
      padding: 3px 8px;
      border-radius: 6px;
      margin-bottom: 4px;
    }

    .artefact-title {
      font-family: var(--font-display, sans-serif);
      font-size: 16px;
      font-weight: 700;
      color: #111827;
      margin: 2px 0 6px;
    }

    .artefact-meta {
      display: flex;
      flex-wrap: wrap;
      gap: 6px;
    }

    .meta-tag {
      font-size: 11px;
      background: #f1f3f5;
      color: #4b5563;
      padding: 3px 8px;
      border-radius: 6px;
      border: 1px solid #e5e7eb;
    }

    .viewer-controls {
      display: flex;
      gap: 6px;
    }

    .control-btn {
      display: inline-flex;
      align-items: center;
      gap: 5px;
      background: #ffffff;
      border: 1px solid #d1d5db;
      color: #374151;
      font-size: 11.5px;
      font-weight: 600;
      padding: 6px 12px;
      border-radius: 8px;
      cursor: pointer;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
      transition: all 0.2s ease;
    }

    .control-btn:hover {
      background: #f9fafb;
      border-color: #111827;
      color: #111827;
    }

    .control-btn.active {
      background: #111827;
      border-color: #111827;
      color: #ffffff;
    }

    .canvas-wrapper {
      position: relative;
      width: 100%;
      height: 330px;
      background: radial-gradient(circle at center, #ffffff 0%, #f1f5f9 100%);
      cursor: grab;
      overflow: hidden;
    }

    .canvas-wrapper:active {
      cursor: grabbing;
    }

    .canvas-hint {
      position: absolute;
      bottom: 10px;
      left: 14px;
      font-size: 11px;
      color: #9ca3af;
      pointer-events: none;
      font-family: var(--font-mono, monospace);
    }

    .artefact-discovery-note {
      padding: 12px 18px;
      font-size: 12px;
      color: #4b5563;
      background: #fbfbfa;
      border-top: 1px solid rgba(0, 0, 0, 0.05);
      line-height: 1.5;
    }
  `]
})
export class ArtefactViewer3DComponent implements OnInit, OnChanges, OnDestroy {
  @Input() artefact: Artefact | null = null;
  @ViewChild('canvasContainer', { static: true }) canvasContainerRef!: ElementRef<HTMLDivElement>;

  protected readonly isAutoRotating = signal(true);

  private scene!: THREE.Scene;
  private camera!: THREE.PerspectiveCamera;
  private renderer!: THREE.WebGLRenderer;
  private currentMeshGroup: THREE.Group = new THREE.Group();
  private animationFrameId: number | null = null;

  // Interaction State
  private isDragging = false;
  private previousMousePosition = { x: 0, y: 0 };
  private targetRotation = { x: 0.2, y: 0 };

  ngOnInit(): void {
    this.initThree();
    this.buildArtefactModel(this.artefact?.model3DType || 'seal_cube');
    this.startRenderLoop();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['artefact'] && !changes['artefact'].firstChange) {
      this.buildArtefactModel(this.artefact?.model3DType || 'seal_cube');
    }
  }

  ngOnDestroy(): void {
    if (this.animationFrameId !== null) {
      cancelAnimationFrame(this.animationFrameId);
    }
    if (this.renderer) {
      this.renderer.dispose();
    }
  }

  toggleAutoRotate(): void {
    this.isAutoRotating.update(v => !v);
  }

  resetView(): void {
    this.targetRotation = { x: 0.2, y: 0 };
    if (this.currentMeshGroup) {
      this.currentMeshGroup.rotation.set(0.2, 0, 0);
    }
    this.camera.position.set(0, 0, 4.5);
  }

  onMouseDown(event: MouseEvent): void {
    this.isDragging = true;
    this.previousMousePosition = { x: event.clientX, y: event.clientY };
  }

  onMouseMove(event: MouseEvent): void {
    if (!this.isDragging) return;
    const deltaX = event.clientX - this.previousMousePosition.x;
    const deltaY = event.clientY - this.previousMousePosition.y;

    this.targetRotation.y += deltaX * 0.008;
    this.targetRotation.x += deltaY * 0.008;

    this.previousMousePosition = { x: event.clientX, y: event.clientY };
  }

  onMouseUp(): void {
    this.isDragging = false;
  }

  onWheel(event: WheelEvent): void {
    event.preventDefault();
    const zoomFactor = event.deltaY * 0.003;
    this.camera.position.z = Math.min(8.0, Math.max(2.2, this.camera.position.z + zoomFactor));
  }

  private initThree(): void {
    const container = this.canvasContainerRef.nativeElement;
    const width = container.clientWidth || 400;
    const height = container.clientHeight || 330;

    this.scene = new THREE.Scene();

    this.camera = new THREE.PerspectiveCamera(45, width / height, 0.1, 100);
    this.camera.position.set(0, 0, 4.5);

    this.renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    this.renderer.setSize(width, height);
    this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;

    container.appendChild(this.renderer.domElement);

    // Museum Gallery Studio Lighting (Bright, Crisp, Soft Shadows)
    const ambientLight = new THREE.AmbientLight(0xffffff, 1.1);
    this.scene.add(ambientLight);

    const dirLight1 = new THREE.DirectionalLight(0xfff8ee, 1.4);
    dirLight1.position.set(4, 5, 4);
    dirLight1.castShadow = true;
    this.scene.add(dirLight1);

    const dirLight2 = new THREE.DirectionalLight(0xdbeafe, 0.6);
    dirLight2.position.set(-4, -2, -3);
    this.scene.add(dirLight2);

    const gallerySpot = new THREE.PointLight(0xfff3dc, 1.2, 8);
    gallerySpot.position.set(0, 2, 2.5);
    this.scene.add(gallerySpot);

    this.scene.add(this.currentMeshGroup);
  }

  /**
   * Procedural PBR 3D Generation for Authentic Diagnostic Archaeological Artifacts
   */
  private buildArtefactModel(modelType: string): void {
    while (this.currentMeshGroup.children.length > 0) {
      const obj = this.currentMeshGroup.children[0] as THREE.Mesh;
      if (obj.geometry) obj.geometry.dispose();
      this.currentMeshGroup.remove(obj);
    }

    switch (modelType) {
      case 'seal_cube':
        this.createSteatiteSealModel();
        break;
      case 'bronze_chariot':
        this.createSinauliChariotModel();
        break;
      case 'ashokan_relief':
      case 'stone_stele':
        this.createAshokanReliefModel();
        break;
      case 'sangam_potsherd':
        this.createSangamPotsherdModel();
        break;
      case 'pottery_amphora':
        this.createRomanAmphoraModel();
        break;
      case 'dancing_girl_bronze':
        this.createBronzeFigurineModel();
        break;
      case 'cave_art_slab':
        this.createBhimbetkaCaveSlabModel();
        break;
      default:
        this.createSteatiteSealModel();
        break;
    }
  }

  private createSteatiteSealModel(): void {
    const sealMat = new THREE.MeshStandardMaterial({
      color: 0xdfd7c5,
      roughness: 0.45,
      metalness: 0.1
    });

    const baseGeo = new THREE.BoxGeometry(2.0, 2.0, 0.45);
    const sealMesh = new THREE.Mesh(baseGeo, sealMat);
    this.currentMeshGroup.add(sealMesh);

    const bovineMat = new THREE.MeshStandardMaterial({ color: 0xc8bc9e, roughness: 0.6 });
    const torsoGeo = new THREE.CylinderGeometry(0.35, 0.45, 1.1, 16);
    torsoGeo.rotateZ(Math.PI / 2);
    const torsoMesh = new THREE.Mesh(torsoGeo, bovineMat);
    torsoMesh.position.set(0, -0.15, 0.25);
    this.currentMeshGroup.add(torsoMesh);

    const hornGeo = new THREE.ConeGeometry(0.08, 0.75, 12);
    hornGeo.rotateZ(-Math.PI / 4);
    const hornMesh = new THREE.Mesh(hornGeo, bovineMat);
    hornMesh.position.set(0.4, 0.4, 0.25);
    this.currentMeshGroup.add(hornMesh);

    const standMat = new THREE.MeshStandardMaterial({ color: 0x8a7a5d, roughness: 0.7 });
    const standGeo = new THREE.CylinderGeometry(0.18, 0.22, 0.8, 12);
    const standMesh = new THREE.Mesh(standGeo, standMat);
    standMesh.position.set(0.7, -0.2, 0.25);
    this.currentMeshGroup.add(standMesh);

    const bossGeo = new THREE.CylinderGeometry(0.35, 0.45, 0.35, 16);
    bossGeo.rotateX(Math.PI / 2);
    const bossMat = new THREE.MeshStandardMaterial({ color: 0xb5a88e, roughness: 0.5 });
    const bossMesh = new THREE.Mesh(bossGeo, bossMat);
    bossMesh.position.set(0, 0, -0.35);
    this.currentMeshGroup.add(bossMesh);
  }

  private createSinauliChariotModel(): void {
    const woodMat = new THREE.MeshStandardMaterial({ color: 0x6e482f, roughness: 0.7 });
    const copperMat = new THREE.MeshStandardMaterial({ color: 0xc86432, metalness: 0.75, roughness: 0.35 });

    const axleGeo = new THREE.CylinderGeometry(0.06, 0.06, 2.2, 16);
    axleGeo.rotateZ(Math.PI / 2);
    const axleMesh = new THREE.Mesh(axleGeo, woodMat);
    this.currentMeshGroup.add(axleMesh);

    const wheelGeo = new THREE.CylinderGeometry(0.85, 0.85, 0.12, 32);
    wheelGeo.rotateZ(Math.PI / 2);

    const leftWheel = new THREE.Mesh(wheelGeo, woodMat);
    leftWheel.position.set(-1.0, 0, 0);
    this.currentMeshGroup.add(leftWheel);

    const rightWheel = new THREE.Mesh(wheelGeo, woodMat);
    rightWheel.position.set(1.0, 0, 0);
    this.currentMeshGroup.add(rightWheel);

    const capGeo = new THREE.CylinderGeometry(0.2, 0.2, 0.16, 16);
    capGeo.rotateZ(Math.PI / 2);
    const leftCap = new THREE.Mesh(capGeo, copperMat);
    leftCap.position.set(-1.08, 0, 0);
    this.currentMeshGroup.add(leftCap);

    const rightCap = new THREE.Mesh(capGeo, copperMat);
    rightCap.position.set(1.08, 0, 0);
    this.currentMeshGroup.add(rightCap);

    const chassisGeo = new THREE.BoxGeometry(1.2, 0.1, 1.4);
    const chassisMesh = new THREE.Mesh(chassisGeo, woodMat);
    chassisMesh.position.set(0, 0.25, 0.4);
    this.currentMeshGroup.add(chassisMesh);

    const railGeo = new THREE.CylinderGeometry(0.65, 0.65, 0.6, 16, 1, true, 0, Math.PI);
    railGeo.rotateX(Math.PI / 2);
    const railMesh = new THREE.Mesh(railGeo, copperMat);
    railMesh.position.set(0, 0.55, 0.9);
    this.currentMeshGroup.add(railMesh);

    const poleGeo = new THREE.CylinderGeometry(0.05, 0.05, 2.0, 12);
    poleGeo.rotateX(Math.PI / 2);
    const poleMesh = new THREE.Mesh(poleGeo, woodMat);
    poleMesh.position.set(0, 0.15, 1.8);
    this.currentMeshGroup.add(poleMesh);
  }

  private createAshokanReliefModel(): void {
    const limestoneMat = new THREE.MeshStandardMaterial({
      color: 0xd9dfce,
      roughness: 0.6,
      metalness: 0.05
    });

    const slabGeo = new THREE.BoxGeometry(1.8, 2.4, 0.3);
    const slabMesh = new THREE.Mesh(slabGeo, limestoneMat);
    this.currentMeshGroup.add(slabMesh);

    const figureMat = new THREE.MeshStandardMaterial({ color: 0xc4cbba, roughness: 0.7 });
    const torsoGeo = new THREE.CylinderGeometry(0.3, 0.38, 0.9, 16);
    const torsoMesh = new THREE.Mesh(torsoGeo, figureMat);
    torsoMesh.position.set(0, 0.1, 0.2);
    this.currentMeshGroup.add(torsoMesh);

    const headGeo = new THREE.SphereGeometry(0.24, 16, 16);
    const headMesh = new THREE.Mesh(headGeo, figureMat);
    headMesh.position.set(0, 0.75, 0.22);
    this.currentMeshGroup.add(headMesh);

    const turbanGeo = new THREE.TorusGeometry(0.24, 0.08, 12, 24);
    turbanGeo.rotateX(Math.PI / 2);
    const turbanMesh = new THREE.Mesh(turbanGeo, figureMat);
    turbanMesh.position.set(0, 0.85, 0.22);
    this.currentMeshGroup.add(turbanMesh);
  }

  private createSangamPotsherdModel(): void {
    const sherdGeo = new THREE.CylinderGeometry(1.4, 1.3, 1.5, 24, 1, true, 0, Math.PI / 2);
    const ceramicMat = new THREE.MeshStandardMaterial({
      color: 0x933b27,
      roughness: 0.55,
      side: THREE.DoubleSide
    });
    const sherdMesh = new THREE.Mesh(sherdGeo, ceramicMat);
    sherdMesh.rotation.set(0.4, 0.3, -0.2);
    this.currentMeshGroup.add(sherdMesh);
  }

  private createRomanAmphoraModel(): void {
    const amphoraMat = new THREE.MeshStandardMaterial({ color: 0xd2a679, roughness: 0.65 });

    const bodyGeo = new THREE.CylinderGeometry(0.15, 0.65, 1.8, 20);
    const bodyMesh = new THREE.Mesh(bodyGeo, amphoraMat);
    this.currentMeshGroup.add(bodyMesh);

    const neckGeo = new THREE.CylinderGeometry(0.2, 0.25, 0.7, 16);
    const neckMesh = new THREE.Mesh(neckGeo, amphoraMat);
    neckMesh.position.set(0, 1.1, 0);
    this.currentMeshGroup.add(neckMesh);

    const handleMat = new THREE.MeshStandardMaterial({ color: 0xba8c60, roughness: 0.7 });
    const handleGeo = new THREE.TorusGeometry(0.35, 0.06, 12, 24, Math.PI);
    handleGeo.rotateZ(Math.PI / 2);

    const leftHandle = new THREE.Mesh(handleGeo, handleMat);
    leftHandle.position.set(-0.35, 0.95, 0);
    this.currentMeshGroup.add(leftHandle);

    const rightHandle = new THREE.Mesh(handleGeo, handleMat);
    rightHandle.rotation.set(0, Math.PI, Math.PI / 2);
    rightHandle.position.set(0.35, 0.95, 0);
    this.currentMeshGroup.add(rightHandle);
  }

  private createBronzeFigurineModel(): void {
    const bronzeMat = new THREE.MeshStandardMaterial({
      color: 0x475549,
      metalness: 0.8,
      roughness: 0.35
    });

    const torsoGeo = new THREE.CylinderGeometry(0.14, 0.18, 1.2, 16);
    const torsoMesh = new THREE.Mesh(torsoGeo, bronzeMat);
    this.currentMeshGroup.add(torsoMesh);

    const headGeo = new THREE.SphereGeometry(0.18, 16, 16);
    const headMesh = new THREE.Mesh(headGeo, bronzeMat);
    headMesh.position.set(0, 0.8, 0);
    this.currentMeshGroup.add(headMesh);

    const bunGeo = new THREE.SphereGeometry(0.14, 16, 16);
    bunGeo.scale(1.3, 0.9, 0.9);
    const bunMesh = new THREE.Mesh(bunGeo, bronzeMat);
    bunMesh.position.set(0.22, 0.82, -0.05);
    this.currentMeshGroup.add(bunMesh);

    const bangleArmGeo = new THREE.CylinderGeometry(0.12, 0.08, 0.9, 12);
    bangleArmGeo.rotateZ(-Math.PI / 6);
    const armMesh = new THREE.Mesh(bangleArmGeo, bronzeMat);
    armMesh.position.set(-0.35, 0.25, 0.1);
    this.currentMeshGroup.add(armMesh);
  }

  private createBhimbetkaCaveSlabModel(): void {
    const rockMat = new THREE.MeshStandardMaterial({ color: 0xb58a63, roughness: 0.9 });
    const slabGeo = new THREE.BoxGeometry(2.4, 1.8, 0.35);
    const slabMesh = new THREE.Mesh(slabGeo, rockMat);
    this.currentMeshGroup.add(slabMesh);

    const paintMat = new THREE.MeshStandardMaterial({ color: 0x8b1e0f, roughness: 0.95 });
    const bisonGeo = new THREE.CylinderGeometry(0.35, 0.45, 0.9, 12);
    bisonGeo.rotateZ(Math.PI / 2);
    const bisonMesh = new THREE.Mesh(bisonGeo, paintMat);
    bisonMesh.position.set(-0.2, 0.1, 0.19);
    this.currentMeshGroup.add(bisonMesh);
  }

  private startRenderLoop(): void {
    const animate = () => {
      this.animationFrameId = requestAnimationFrame(animate);

      if (this.isAutoRotating() && !this.isDragging) {
        this.targetRotation.y += 0.008;
      }

      if (this.currentMeshGroup) {
        this.currentMeshGroup.rotation.y += (this.targetRotation.y - this.currentMeshGroup.rotation.y) * 0.08;
        this.currentMeshGroup.rotation.x += (this.targetRotation.x - this.currentMeshGroup.rotation.x) * 0.08;
      }

      this.renderer.render(this.scene, this.camera);
    };

    animate();
  }
}
