import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/TelasIniciais/HomeInicial.vue'
import { isAuthenticated } from '@/services/auth.js';

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
      meta: { requiresAuth: true }
    },
    {
      path: '/lista',
      name: 'listaCriar',
      component: () => import('../views/FluxoPrincipal/ListaPrincipal.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/lista/:id',
      name: 'listaEditar',
      component: () => import('../views/FluxoPrincipal/ListaPrincipal.vue'),
      meta: { requiresAuth: true }
    },
  ],
})

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !isAuthenticated()) {
    next({ path: '/login' });
  } else {
    next();
  }
});

export default router
