import {createRouter, createWebHistory} from 'vue-router'
import Authorize from "@/components/Authorize.vue";
import Courses from "@/components/Courses.vue";

const routes = [
    {
        path: '/',
        name: 'Courses',
        component: Courses
    },
    {
        path: '/auth',
        name: 'Authorize',
        component: Authorize
    },
]
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes
})
export default router