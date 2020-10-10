<template>
  <nav class="navbar">
    <div class="logo-container">
      <img src="~assets/logo-islas-canarias.webp"></img>
    </div>
    <div class="links-container">
      <ul>
        <li>Cómo funciona</li>
        <li>Nosotros</li>
        <li>Contacto</li>
      </ul>
    </div>
  </nav>
</template>

<script lang="ts">
export default {
  name: 'Navbar',
  data () {
    return {}
  },
  computed: {},
  methods: {}
}
</script>

<style>
  .navbar {
    display: flex;
    width: 100%;
    height: 8vh;
  }

  .logo-container {
    flex: 0 0 20%;
    height: inherit;
    padding: 0 2vw;

    display: flex;
    align-content: center;
    justify-content: center;
  }

  .links-container {
    flex: 1 0 auto;
    height: inherit;
    padding: 2vh 2vw;

    display: flex;
    align-content: center;
    justify-content: flex-end;
  }

  .links-container li {
     display:inline;
     font-size: 24px;
     padding: 1vw;
  }
</style>
