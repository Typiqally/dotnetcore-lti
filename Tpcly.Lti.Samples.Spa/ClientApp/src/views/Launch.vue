<template>
  <div>Launch {{token}}</div>
</template>

<script setup lang="ts">
import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";

const route = useRoute()
const token = ref()

onMounted(async () => {
  const credentials = await getCredentials()
  console.log(credentials)
})

async function getCredentials() {
  console.log(JSON.stringify({
    "token": route.query.token,
  }))
  
  try {
    let response = await fetch('https://localhost:7084/lti/exchange', {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        "token": route.query.token,
      })
    });
    
    if (response.ok) {
      return await response.json();
    }
  } catch (error: any) {
    console.error(error)
  }
}
</script>

<style scoped>
button {
  background: #535bf2;
}
</style>