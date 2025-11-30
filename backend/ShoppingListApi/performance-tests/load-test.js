import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

const errorRate = new Rate('errors');

export const options = {
  stages: [
    { duration: '30s', target: 500 },    // Rampa inicial: 0 -> 500 usuários em 30s
    { duration: '30s', target: 1000 },   // Aumento: 500 -> 1000 usuários em 30s
    { duration: '1m', target: 1000 },    // Manter 1000 usuários por 1 minuto
    { duration: '30s', target: 0 },      // Rampa de descida: 1000 -> 0 em 30s
  ],
  thresholds: {
    'errors': ['rate<0.1'],            // Taxa de erro deve ser menor que 10%
    'http_req_duration': ['p(95)<2000'], // 95% das requisições devem completar em menos de 2s
    'http_req_duration': ['p(99)<3000'], // 99% das requisições devem completar em menos de 3s
  },
  // Configurações para otimizar o teste
  noConnectionReuse: false,             // Permite reutilização de conexões
  userAgent: 'K6 Load Test',           // Identifica o teste de carga
  // Configurações de batch
  batchPerHost: 6,                     // Número de requisições simultâneas por host
  // Configurações de timeout
  setupTimeout: '30s',
  teardownTimeout: '30s'
};

const BASE_URL = 'http://localhost:5139';

// Função auxiliar para fazer login
function login() {
  const loginRes = http.post(`${BASE_URL}/api/Auth/login`, JSON.stringify({
    email: 'test@example.com',
    senha: 'Test@123'
  }), {
    headers: { 'Content-Type': 'application/json' },
  });

  check(loginRes, {
    'login successful': (r) => r.status === 200,
    'has token': (r) => JSON.parse(r.body).token !== undefined,
  });

  return JSON.parse(loginRes.body).token;
}

export default function () {
  // Login e obtenção do token
  const token = login();
  
  // Se o login falhou, não continua com os outros testes
  if (!token) {
    errorRate.add(1);
    return;
  }

  // Configuração do header de autorização
  const params = {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
  };

  // Teste: Criar uma nova lista
  const newList = {
    nomeLista: `Lista Teste ${Date.now()}`,
  };

  const createListRes = http.post(`${BASE_URL}/api/lista`, JSON.stringify(newList), params);
  check(createListRes, {
    'create list successful': (r) => r.status === 201,
  });

  // Extrair o ID da lista criada
  const listaId = JSON.parse(createListRes.body).idLista;
  console.log('ID da lista criada:', listaId);

  // Teste: Listar todos os itens da lista
  console.log('Tentando listar itens para a lista:', listaId);
  console.log('Headers:', JSON.stringify(params.headers));

  const itemsRes = http.get(`${BASE_URL}/api/lista/${listaId}/Item`, params);
  
  // Log detalhado da resposta
  console.log('=== Detalhes da Requisição GET Itens ===');
  console.log('URL:', `${BASE_URL}/api/lista/${listaId}/Item`);
  console.log('Status:', itemsRes.status);
  console.log('Status Text:', itemsRes.status_text);
  console.log('Headers:', JSON.stringify(itemsRes.headers));
  console.log('Body:', itemsRes.body);
  if (itemsRes.error) {
    console.log('Erro na requisição:', itemsRes.error);
  }

  check(itemsRes, {
    'get items successful': (r) => {
      if (r.status !== 200) {
        console.log(`Falha no GET: Status ${r.status}, Body: ${r.body}`);
      }
      return r.status === 200;
    },
    'items response is array': (r) => {
      try {
        const parsed = JSON.parse(r.body);
        const isArray = Array.isArray(parsed);
        if (!isArray) {
          console.log('Resposta não é um array:', typeof parsed, r.body);
        }
        return isArray;
      } catch (e) {
        console.log('Erro ao fazer parse da resposta:', e.message);
        console.log('Body recebido:', r.body);
        return false;
      }
    },
  });

  // Teste: Criar um novo item na lista
  const newItem = {
    nomeItem: `Item Teste ${Date.now()}`,
    quantidade: Math.floor(Math.random() * 10) + 1,
  };

  const createItemRes = http.post(`${BASE_URL}/api/lista/${listaId}/Item`, JSON.stringify(newItem), params);
  check(createItemRes, {
    'create item successful': (r) => r.status === 201,
  });

  // Pausa entre iterações para simular comportamento do usuário
  sleep(1);
}
