<template>
    <h1>Courses</h1>
    <ul v-if="courses">
        <li v-for="course in courses">{{ course.name }}</li>
    </ul>
</template>

<script setup lang="ts">
import {onMounted, ref} from "vue";
import router from "../router";

const courses = ref([])

onMounted(async () => {
    courses.value = await getCourses()
})

function getCourses() {
    return fetch('https://localhost:7084/api/course', {credentials: "include"})
        .then(response => {
            if (response.ok) {
                return response.json()
            }

            return Promise.reject(response);
        })
        .then(data => data)
        .catch(error => {
            if (error.status == 401) {
                router.push('/auth')
            }
        })
}
</script>

<style scoped>
button {
    background: #535bf2;
}
</style>