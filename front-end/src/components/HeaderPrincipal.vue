<template>
    <div class="w-100 d-flex justify-content-between align-items-center mb-6" v-if="telaHome">
        <span>{{ model.usuario }}</span>
        <span @click="logout()" class="textoLink">Sair</span>
    </div>
    <div class="w-100 d-flex justify-content-between align-items-center mb-6" v-else-if="telaExpandida">
        <img src="../assets/imgs/CartHeader.svg" alt="Logo" @click="goHome()" class="h-75">
        <slot name="right"></slot>
    </div>
    <div class="w-100 d-flex justify-content-between align-items-center mb-6" v-else>
        <img src="../assets/imgs/CartHeader.svg" alt="Logo" @click="goHome()" class="h-75">
        <span @click="goHome()" class="textoLink">Listas</span>
    </div>
</template>

<script>
import { logout } from '@/services/auth.js';
import { useUserStore } from '@/stores/user.js'

export default {
    props: {
        telaHome: { type: Boolean, default: false },
        telaExpandida: { type: Boolean, default: false },
    },
    computed: {
        model () {
            return useUserStore()
        }
    },
    methods: {
        logout () {
            logout()
            const userStore = useUserStore()
            userStore.clearUser()
            this.$router.push('/')
        },
        goHome () {
            this.$router.push('/home')
        }
    }
}
</script>