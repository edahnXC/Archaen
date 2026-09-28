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
          <span class="badge-3d">3D WebGL PBR Lab</span>
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
      background: #11141c;
      border: 1px solid rgba(212, 175, 55, 0.25);
      border-radius: 12px;
      overflow: hidden;
      box-shadow: 0 12px 30px rgba(0, 0, 0, 0.5);
    }

    .viewer-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      padding: 14px 18px;
      background: rgba(22, 26, 36, 0.9);
      border-bottom: 1px solid rgba(255, 255, 255, 0.07);
    }

    .badge-3d {
      display: inline-block;
      font-size: 10px;
      font-weight: 700;
      letter-spacing: 0.08em;
      text-transform: uppercase;
      color: #e9c46a;
      background: rgba(233, 196, 106, 0.15);
      border: 1px solid rgba(233, 196, 106, 0.3);
      padding: 2px 7px;
      border-radius: 4px;
      margin-bottom: 4px;
    }

    .artefact-title {
      font-family: var(--font-display, serif);
      font-size: 15px;
      color: #f3f4f6;
      margin: 2px 0 6px;
    }

    .artefact-meta {
      display: flex;
      flex-wrap: wrap;
      gap: 6px;
    }

    .meta-tag {
      font-size: 11px;
      background: rgba(255, 255, 255, 0.06);
      color: #9ca3af;
      padding: 2px 8px;
      border-radius: 4px;
      border: 1px solid rgba(255, 255, 255, 0.06);
    }

    .viewer-controls {
      display: flex;
      gap: 6px;
    }

    .control-btn {
      display: inline-flex;
      align-items: center;
      gap: 5px;
      background: rgba(255, 255, 255, 0.06);
      border: 1px solid rgba(255, 255, 255, 0.12);
      color: #e5e7eb;
      font-size: 11px;
      font-weight: 600;
      padding: 5px 10px;
      border-radius: 6px;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .control-btn:hover {
      background: rgba(212, 175, 55, 0.2);
      border-color: #d4af37;
      color: #ffd166;
    }

    .control-btn.active {
      background: rgba(212, 175, 55, 0.25);
      border-color: #d4af37;
      color: #ffd166;
    }

    .canvas-wrapper {
      position: relative;
      width: 100%;
      height: 320px;
      background: radial-gradient(circle at center, #1b202e 0%, #0d0f14 100%);
      cursor: grab;
      overflow: hidden;
    }

    .canvas-wrapper:active {
      cursor: grabbing;
    }

    .canvas-hint {
      position: absolute;
      bottom: 8px;
      left: 12px;
      font-size: 10px;
      color: rgba(255, 255, 255, 0.35);
      pointer-events: none;
      font-family: var(--font-mono, monospace);
    }

    .artefact-discovery-note {
      padding: 10px 16px;
      font-size: 12px;
      color: #94a3b8;
      background: rgba(15, 18, 26, 0.7);
      border-top: 1px solid rgba(255, 255, 255, 0.05);
      line-height: 1.4;
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
    const height = container.clientHeight || 320;

    this.scene = new THREE.Scene();

    this.camera = new THREE.PerspectiveCamera(45, width / height, 0.1, 100);
    this.camera.position.set(0, 0, 4.5);

    this.renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    this.renderer.setSize(width, height);
    this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;

    container.appendChild(this.renderer.domElement);

    // Dynamic Archaeological Lighting setup
    const ambientLight = new THREE.AmbientLight(0xfff5e6, 0.85);
    this.scene.add(ambientLight);

    const dirLight1 = new THREE.DirectionalLight(0xffd7a0, 1.4);
    dirLight1.position.set(4, 5, 4);
    dirLight1.castShadow = true;
    this.scene.add(dirLight1);

    const dirLight2 = new THREE.DirectionalLight(0x7090b0, 0.7);
    dirLight2.position.set(-4, -2, -3);
    this.scene.add(dirLight2);

    const goldPoint = new THREE.PointLight(0xd4af37, 1.2, 8);
    goldPoint.position.set(0, 2, 2.5);
    this.scene.add(goldPoint);

    this.scene.add(this.currentMeshGroup);
  }

  /**
   * Procedural PBR 3D Generation for Authentic Diagnostic Archaeological Artifacts
   */
  private buildArtefactModel(modelType: string): void {
    // Clear previous geometries
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

  /**
   * Harappan Steatite Unicorn Seal
   */
  private createSteatiteSealModel(): void {
    const sealMat = new THREE.MeshStandardMaterial({
      color: 0xdfd7c5, // Bleached steatite
      roughness: 0.45,
      metalness: 0.1
    });

    // Square seal block
    const baseGeo = new THREE.BoxGeometry(2.0, 2.0, 0.45);
    const sealMesh = new THREE.Mesh(baseGeo, sealMat);
    this.currentMeshGroup.add(sealMesh);

    // Intaglio Unicorn / Sacred Bovine Relief
    const bovineMat = new THREE.MeshStandardMaterial({
      color: 0xc8bc9e,
      roughness: 0.6
    });
    const torsoGeo = new THREE.CylinderGeometry(0.35, 0.45, 1.1, 16);
    torsoGeo.rotateZ(Math.PI / 2);
    const torsoMesh = new THREE.Mesh(torsoGeo, bovineMat);
    torsoMesh.position.set(0, -0.15, 0.25);
    this.currentMeshGroup.add(torsoMesh);

    // Single graceful curved horn
    const hornGeo = new THREE.ConeGeometry(0.08, 0.75, 12);
    hornGeo.rotateZ(-Math.PI / 4);
    const hornMesh = new THREE.Mesh(hornGeo, bovineMat);
    hornMesh.position.set(0.4, 0.4, 0.25);
    this.currentMeshGroup.add(hornMesh);

    // Standard Incense Burner / Offering Stand
    const standMat = new THREE.MeshStandardMaterial({ color: 0x8a7a5d, roughness: 0.7 });
    const standGeo = new THREE.CylinderGeometry(0.18, 0.22, 0.8, 12);
    const standMesh = new THREE.Mesh(standGeo, standMat);
    standMesh.position.set(0.7, -0.2, 0.25);
    this.currentMeshGroup.add(standMesh);

    // Perforated Boss handle on reverse
    const bossGeo = new THREE.CylinderGeometry(0.35, 0.45, 0.35, 16);
    bossGeo.rotateX(Math.PI / 2);
    const bossMat = new THREE.MeshStandardMaterial({ color: 0xb5a88e, roughness: 0.5 });
    const bossMesh = new THREE.Mesh(bossGeo, bossMat);
    bossMesh.position.set(0, 0, -0.35);
    this.currentMeshGroup.add(bossMesh);
  }

  /**
   * Sinauli Royal Copper-Bronze War Chariot with Solid Wheels
   */
  private createSinauliChariotModel(): void {
    const woodMat = new THREE.MeshStandardMaterial({ color: 0x5a3d28, roughness: 0.7 });
    const copperMat = new THREE.MeshStandardMaterial({ color: 0xc86432, metalness: 0.75, roughness: 0.35 });

    // Axle
    const axleGeo = new THREE.CylinderGeometry(0.06, 0.06, 2.2, 16);
    axleGeo.rotateZ(Math.PI / 2);
    const axleMesh = new THREE.Mesh(axleGeo, woodMat);
    this.currentMeshGroup.add(axleMesh);

    // 2 Solid Disk Wheels with Copper Triangle Inlays
    const wheelGeo = new THREE.CylinderGeometry(0.85, 0.85, 0.12, 32);
    wheelGeo.rotateZ(Math.PI / 2);

    const leftWheel = new THREE.Mesh(wheelGeo, woodMat);
    leftWheel.position.set(-1.0, 0, 0);
    this.currentMeshGroup.add(leftWheel);

    const rightWheel = new THREE.Mesh(wheelGeo, woodMat);
    rightWheel.position.set(1.0, 0, 0);
    this.currentMeshGroup.add(rightWheel);

    // Copper Hubcaps
    const capGeo = new THREE.CylinderGeometry(0.2, 0.2, 0.16, 16);
    capGeo.rotateZ(Math.PI / 2);
    const leftCap = new THREE.Mesh(capGeo, copperMat);
    leftCap.position.set(-1.08, 0, 0);
    this.currentMeshGroup.add(leftCap);

    const rightCap = new THREE.Mesh(capGeo, copperMat);
    rightCap.position.set(1.08, 0, 0);
    this.currentMeshGroup.add(rightCap);

    // High Canopy Chassis Platform
    const chassisGeo = new THREE.BoxGeometry(1.2, 0.1, 1.4);
    const chassisMesh = new THREE.Mesh(chassisGeo, woodMat);
    chassisMesh.position.set(0, 0.25, 0.4);
    this.currentMeshGroup.add(chassisMesh);

    // Curved Protective Front Railing with Copper Trimming
    const railGeo = new THREE.CylinderGeometry(0.65, 0.65, 0.6, 16, 1, true, 0, Math.PI);
    railGeo.rotateX(Math.PI / 2);
    const railMesh = new THREE.Mesh(railGeo, copperMat);
    railMesh.position.set(0, 0.55, 0.9);
    this.currentMeshGroup.add(railMesh);

    // Draft Pole
    const poleGeo = new THREE.CylinderGeometry(0.05, 0.05, 2.0, 12);
    poleGeo.rotateX(Math.PI / 2);
    const poleMesh = new THREE.Mesh(poleGeo, woodMat);
    poleMesh.position.set(0, 0.15, 1.8);
    this.currentMeshGroup.add(poleMesh);
  }

  /**
   * Kanaganahalli Ashokan Limestone Relief Slab (Ranyo Asoko)
   */
  private createAshokanReliefModel(): void {
    const limestoneMat = new THREE.MeshStandardMaterial({
      color: 0xd9dfce, // Pale greenish Palnad limestone
      roughness: 0.6,
      metalness: 0.05
    });

    const slabGeo = new THREE.BoxGeometry(1.8, 2.4, 0.3);
    const slabMesh = new THREE.Mesh(slabGeo, limestoneMat);
    this.currentMeshGroup.add(slabMesh);

    // Carved Imperial Torso & Head
    const figureMat = new THREE.MeshStandardMaterial({ color: 0xc4cbba, roughness: 0.7 });
    const torsoGeo = new THREE.CylinderGeometry(0.3, 0.38, 0.9, 16);
    const torsoMesh = new THREE.Mesh(torsoGeo, figureMat);
    torsoMesh.position.set(0, 0.1, 0.2);
    this.currentMeshGroup.add(torsoMesh);

    // Royal Turban / Headgear
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

  /**
   * Sangam Inscribed Black-and-Red Ware Potsherd (Aadhan)
   */
  private createSangamPotsherdModel(): void {
    // Curved ceramic fragment
    const sherdGeo = new THREE.CylinderGeometry(1.4, 1.3, 1.5, 24, 1, true, 0, Math.PI / 2);
    const ceramicMat = new THREE.MeshStandardMaterial({
      color: 0x933b27, // Burnished Terracotta Red Slip
      roughness: 0.55,
      side: THREE.DoubleSide
    });
    const sherdMesh = new THREE.Mesh(sherdGeo, ceramicMat);
    sherdMesh.rotation.set(0.4, 0.3, -0.2);
    this.currentMeshGroup.add(sherdMesh);
  }

  /**
   * Roman Mediterranean Wine Transport Amphora (Arikamedu)
   */
  private createRomanAmphoraModel(): void {
    const amphoraMat = new THREE.MeshStandardMaterial({
      color: 0xd2a679, // Italian coarse terracotta
      roughness: 0.65
    });

    // Body
    const bodyGeo = new THREE.CylinderGeometry(0.15, 0.65, 1.8, 20);
    const bodyMesh = new THREE.Mesh(bodyGeo, amphoraMat);
    this.currentMeshGroup.add(bodyMesh);

    // Neck
    const neckGeo = new THREE.CylinderGeometry(0.2, 0.25, 0.7, 16);
    const neckMesh = new THREE.Mesh(neckGeo, amphoraMat);
    neckMesh.position.set(0, 1.1, 0);
    this.currentMeshGroup.add(neckMesh);

    // Dual handles
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

  /**
   * Mohenjo-daro Dancing Girl Lost-Wax Cast Bronze
   */
  private createBronzeFigurineModel(): void {
    const bronzeMat = new THREE.MeshStandardMaterial({
      color: 0x475549, // Dark antique bronze patina with green verdigris undertones
      metalness: 0.8,
      roughness: 0.35
    });

    // Slender torso
    const torsoGeo = new THREE.CylinderGeometry(0.14, 0.18, 1.2, 16);
    const torsoMesh = new THREE.Mesh(torsoGeo, bronzeMat);
    this.currentMeshGroup.add(torsoMesh);

    // Head with characteristic heavy side chignon
    const headGeo = new THREE.SphereGeometry(0.18, 16, 16);
    const headMesh = new THREE.Mesh(headGeo, bronzeMat);
    headMesh.position.set(0, 0.8, 0);
    this.currentMeshGroup.add(headMesh);

    const bunGeo = new THREE.SphereGeometry(0.14, 16, 16);
    bunGeo.scale(1.3, 0.9, 0.9);
    const bunMesh = new THREE.Mesh(bunGeo, bronzeMat);
    bunMesh.position.set(0.22, 0.82, -0.05);
    this.currentMeshGroup.add(bunMesh);

    // Left arm loaded with 24 bangles
    const bangleArmGeo = new THREE.CylinderGeometry(0.12, 0.08, 0.9, 12);
    bangleArmGeo.rotateZ(-Math.PI / 6);
    const armMesh = new THREE.Mesh(bangleArmGeo, bronzeMat);
    armMesh.position.set(-0.35, 0.25, 0.1);
    this.currentMeshGroup.add(armMesh);
  }

  /**
   * Bhimbetka Prehistoric Zoo Rock Slab
   */
  private createBhimbetkaCaveSlabModel(): void {
    const rockMat = new THREE.MeshStandardMaterial({
      color: 0xb58a63, // Natural Vindhyan sandstone
      roughness: 0.9
    });

    const slabGeo = new THREE.BoxGeometry(2.4, 1.8, 0.35);
    const slabMesh = new THREE.Mesh(slabGeo, rockMat);
    this.currentMeshGroup.add(slabMesh);

    // Stylized Bison in Red Hematite Pigment
    const paintMat = new THREE.MeshStandardMaterial({
      color: 0x8b1e0f, // Deep mineral hematite red
      roughness: 0.95
    });

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

      // Smooth spherical interpolation
      if (this.currentMeshGroup) {
        this.currentMeshGroup.rotation.y += (this.targetRotation.y - this.currentMeshGroup.rotation.y) * 0.08;
        this.currentMeshGroup.rotation.x += (this.targetRotation.x - this.currentMeshGroup.rotation.x) * 0.08;
      }

      this.renderer.render(this.scene, this.camera);
    };

    animate();
  }
}
