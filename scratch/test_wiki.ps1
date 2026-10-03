$headers = @{ "User-Agent" = "ArchaeologicalEducationalBot/1.0 (contact: info@archaeology.edu)" }
$res = Invoke-RestMethod -Uri "https://en.wikipedia.org/api/rest_v1/page/summary/Pashupati_seal" -Headers $headers -TimeoutSec 15
Write-Output $res.originalimage.source
