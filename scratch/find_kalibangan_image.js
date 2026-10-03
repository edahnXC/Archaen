const https = require('https');

function fetchJson(url) {
  return new Promise((resolve, reject) => {
    https.get(url, { headers: { 'User-Agent': 'ArchaeologicalBot/1.0 (contact: info@archaeology.edu)' }, rejectUnauthorized: false }, (res) => {
      let data = '';
      res.on('data', chunk => data += chunk);
      res.on('end', () => {
        if (res.statusCode !== 200) {
          console.log('Status:', res.statusCode, data.substring(0, 200));
          return reject(new Error('Status ' + res.statusCode));
        }
        try {
          resolve(JSON.parse(data));
        } catch (e) {
          console.log('Failed to parse:', data.substring(0, 300));
          reject(e);
        }
      });
    }).on('error', reject);
  });
}

async function run() {
  const url = 'https://en.wikipedia.org/api/rest_v1/page/summary/Kalibangan';
  const data = await fetchJson(url);
  console.log('Title:', data.title);
  console.log('Original image:', data.originalimage);
}

run().catch(console.error);
