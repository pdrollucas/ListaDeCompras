import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/TelasIniciais/HomeInicial.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'homeLogin',
      component: HomeView,
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/TelasIniciais/LoginInicial.vue'),
    },
    {
      path: '/cadastrar',
      name: 'cadastrar',
      component: () => import('../views/TelasIniciais/CadastrarInicial.vue'),
    },
    {
      path: '/home',
      name: 'homePrincipal',
      component: () => import('../views/FluxoPrincipal/HomePrincipal.vue'),
    },
  ],
})

export default router
