<template>
  <div>Launch, with token {{token}}</div>
</template>

<script setup lang="ts">
import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";

const route = useRoute()
const token = ref()

onMounted(async () => {
  token.value = route.query.token
})
</script>

<style scoped>
button {
  background: #535bf2;
}
</style>