<template>
    <button v-on:click="openWindow">Click here to authorize</button>
</template>

<script setup lang="ts">
import {ref} from "vue";
import router from "../router";

const windowRef = ref()

function openWindow() {
    windowRef.value = window.open(
        "https://localhost:7084/login/challenge",
        "",
        "width=600,height=400"
    );

    let timer = setInterval(() => {
        console.log("check")
        if (windowRef.value.closed) {
            router.push("/")
            clearInterval(timer)
        }
    }, 500);
}
</script>

<style scoped>
button {
    background: #535bf2;
}
</style>