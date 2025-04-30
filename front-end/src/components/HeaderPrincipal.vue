<template>
    <div class="w-100 d-flex justify-content-between align-items-center mb-10" v-if="telaHome">
        <span>Usuário</span>
        <span @click="logout()" class="textoLink">Sair</span>
    </div>
    <div class="w-100 d-flex justify-content-between align-items-center mb-10" v-else>
        <img src="../assets/imgs/CartHeader.svg" alt="Logo" @click="goHome()" class="h-75">
        <span @click="goHome()" class="textoLink">Listas</span>
    </div>
</template>

<script>
export default {
    props: {
        telaHome: { type: Boolean, default: false }
    },
    methods: {
        logout () {
            this.$router.push('/')
        },
        goHome () {
            this.$router.push('/home')
        }
    }
}
</script>