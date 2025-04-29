<template>
    <div class="device py-10 px-8">
        <div class="w-100 d-flex justify-content-between mb-10">
            <span>Usuário</span>
            <span @click="logout()" class="textoLink">Sair</span>
        </div>
        <img src="../../assets/imgs/Cart.svg" alt="Logo" class="my-10">
        <div class="mt-10 mb-10 listas">
            <ul v-for="lista in listas" :key="lista.id">
                <li class="lista py-2 pb-2">{{ lista.text }}</li>
            </ul>
        </div>
        <v-btn class="btnInicial">Adicionar lista</v-btn>
    </div>
</template>

<script>
export default {
    data () {
        return {
            listas: [
                {id: 1, text: 'Lista de Mercado'},
                {id: 2, text: 'Lista de Farmácia'},
                {id: 3, text: 'Lista de Churrasco'},
            ]
        }
    },
    methods: {
        logout () {
            this.$router.push('/')
        }
    }
}
</script>

<style scoped>
img {
  height: 20dvh;
}

.listas {
    height: 35dvh;
    overflow-y: auto;
}

.lista {
    background-color: #DEDEDE;
    width: 75dvw;
    text-align: center;
}
</style>