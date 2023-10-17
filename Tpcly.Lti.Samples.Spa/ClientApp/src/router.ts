import {createRouter, createWebHistory} from 'vue-router'
import Index from "@/views/auth/Index.vue";
import Launch from "@/views/Launch.vue";

const routes = [
    {
        path: '/',
        name: 'Launch',
        component: Launch
    },
    {
        path: '/auth',
        name: 'Authorize',
        component: Index
    },
]
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes
})
export default router