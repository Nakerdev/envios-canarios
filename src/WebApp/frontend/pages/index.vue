<template>
  <div>
    <Navbar />
    <div>
      <h1>
        Header
      </h1>
    </div>
    <div>
      <h1>
        Formulario
      </h1>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Navbar from '../components/Navbar.vue'

export default Vue.extend({
  components: {
    Navbar
  }
})
</script>

<style>
/* Sample `apply` at-rules with Tailwind CSS
.container {
@apply min-h-screen flex justify-center items-center text-center mx-auto;
}
*/
</style>
