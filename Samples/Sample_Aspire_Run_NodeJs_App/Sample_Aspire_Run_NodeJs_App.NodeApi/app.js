import { env } from 'node:process';
import { createServer } from 'node:http';
import fetch from 'node-fetch';
import express from 'express';
import { createTerminus, HealthCheckError } from '@godaddy/terminus';

const environment = process.env.NODE_ENV || 'development';
const app = express();
const port = env.PORT ?? 8080;

const apiServer = env['services__apiservice__https__0'] ?? env['services__apiservice__http__0'];
const passwordPrefix = ",password=";

if (apiServer === undefined) {
    throw new Error(`Cannot run as apiServer=${apiServer}. Bye!`)
}
console.log(`environment: ${environment}`);
console.log(`apiServer: ${apiServer}`);

app.get('/', async (req, res) => {
    let response = await fetch(`${apiServer}/weatherforecast`);
    let forecasts = await response.json();
    //await cache.set('forecasts', JSON.stringify(forecasts), { 'EX': 5 });
    res.json({ forecasts: forecasts })  
});

app.set('views', './views');
app.set('view engine', 'pug');

const server = createServer(app)

async function healthCheck() {
    const errors = [];
    const apiServerHealthAddress = `${apiServer}/health`;
    console.log(`Fetching ${apiServerHealthAddress}`);
    var response = await fetch(apiServerHealthAddress);
    if (!response.ok) {
        throw new HealthCheckError(`Fetching ${apiServerHealthAddress} failed with HTTP status: ${response.status}`);
    }
}

createTerminus(server, {
    signal: 'SIGINT',
    healthChecks: {
        '/health': healthCheck,
        '/alive': () => { }
    },
    onSignal: async () => {
        console.log('server is starting cleanup');
        console.log('closing Redis connection');
        await cache.disconnect();
    },
    onShutdown: () => console.log('cleanup finished, server is shutting down')
});

server.listen(port, () => {
    console.log(`Listening on port ${port}`);
});