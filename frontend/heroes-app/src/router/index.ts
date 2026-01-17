import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/heroes',
    },
    {
      path: '/heroes',
      name: 'heroes',
      component: () => import('../views/HeroesList.vue'),
    },
    {
      path: '/heroes/create',
      name: 'hero-create',
      component: () => import('../views/HeroForm.vue'),
    },
    {
      path: '/heroes/:id/edit',
      name: 'hero-edit',
      component: () => import('../views/HeroForm.vue'),
    },
    {
      path: '/heroes/:id',
      name: 'hero-detail',
      component: () => import('../views/HeroDetail.vue'),
    },
  ],
})

export default router
