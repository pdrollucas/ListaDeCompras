<template>
    <div class="device py-10 px-8">
        <HeaderPrincipal telaHome/>
        <img src="../../assets/imgs/Cart.svg" alt="Logo" class="my-10">
        <div class="mt-10 mb-10 listas">
            <template v-if="listas && listas.length > 0">
                <ul v-for="lista in listas" :key="lista.idLista">
                    <li class="lista py-4 px-4">
                        <span @click="openLista(lista.idLista)" data-cy="list-item"> {{ lista.nomeLista }}</span>
                        <span @click="removeLista(lista.idLista)" data-cy="delete-list-button">x</span>
                    </li>
                </ul>
            </template>
            <template v-else>
                <p class="lista py-4 px-4 text-center">Você não possui nenhuma lista até o momento</p>
            </template>
        </div>
        <v-btn class="btnInicial" @click="addLista()" data-cy="new-list-button">Adicionar lista</v-btn>
    </div>
</template>

<script>
import HeaderPrincipal from '@/components/HeaderPrincipal.vue';
import { useUserStore } from '@/stores/user'
import { addLista, deleteLista } from '@/services/lista.js'

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
        async addLista() {
            try {
                const novaLista = await addLista('Nova Lista')
                this.userStore.listas.push(novaLista)
                this.$router.push({ name: 'listaEditar', params: { id: novaLista.idLista } })
            } catch (err) {
                alert('Erro ao criar lista: ' + err.message)
            }
        },
        async removeLista(id) {
            if (!confirm('Tem certeza que deseja deletar esta lista?')) return;
            try {
                await deleteLista(id)
                this.userStore.listas = this.userStore.listas.filter(l => l.idLista !== id)
            } catch (err) {
                alert('Erro ao deletar lista: ' + err.message)
            }
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
    display: flex;
    justify-content: space-between;
    align-items: center;
}
</style>