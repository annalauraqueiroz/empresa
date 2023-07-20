<template>
  <Empresa/>
</template>

<script>
    import Empresa from './components/Empresa/Index.vue'
    import Pessoal from './components/Pessoal/Index.vue'

export default {
  name: 'App',
  components: {
      Empresa,
      Pessoal
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
