<template>
    <div class="device py-10 px-8">
        <HeaderPrincipal telaHome/>
        <img src="../../assets/imgs/Cart.svg" alt="Logo" class="my-10">
        <div class="mt-10 mb-10 listas">
            <template v-if="listas && listas.length > 0">
                <ul v-for="lista in listas" :key="lista.idLista">
                    <li class="lista py-2 pb-2" @click="openLista(lista.idLista)">
                        {{ lista.nomeLista }}
                    </li>
                </ul>
            </template>
            <template v-else>
                <p class="lista py-2 pb-2">Você não possui nenhuma lista até o momento</p>
            </template>
        </div>
        <v-btn class="btnInicial" @click="addLista()">Adicionar lista</v-btn>
    </div>
</template>

<script>
import HeaderPrincipal from '@/components/HeaderPrincipal.vue';
import { useUserStore } from '@/stores/user'

export default {
    components: { HeaderPrincipal },
    data() {
        return {
            userStore: useUserStore()
        }
    },
    computed: {
        listas() {
            return this.userStore.listas
        }
    },
    async mounted() {
        await this.userStore.fetchListas()
    },
    methods: {
        openLista(id) {
            this.$router.push({ name: 'listaEditar', params: { id } })
        },
        addLista() {
            this.$router.push({ name: 'listaCriar' })
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