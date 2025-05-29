<template>
    <div class="device py-10 px-8">
        <v-snackbar
            v-model="snackbar.show"
            :color="snackbar.color"
            :timeout="snackbar.timeout"
            location="top"
        >
            {{ snackbar.text }}
            <template v-if="snackbar.showAction" v-slot:actions>
                <v-btn
                    :color="snackbar.actionColor"
                    @click="snackbar.onAction"
                    variant="text"
                >
                    {{ snackbar.actionText }}
                </v-btn>
                <v-btn
                    color="white"
                    @click="snackbar.show = false"
                    variant="text"
                >
                    Cancelar
                </v-btn>
            </template>
        </v-snackbar>
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
            userStore: useUserStore(),
            snackbar: {
                show: false,
                text: '',
                color: 'success',
                timeout: 4000,
                showAction: false,
                actionText: '',
                actionColor: '',
                onAction: null
            },
            listaParaExcluir: null
        }
    },
    computed: {
        listas() {
            return this.userStore.listas
        }
    },
    async mounted() {
        try {
            await this.userStore.fetchListas()
        } catch (err) {
            this.snackbar = {
                show: true,
                text: err.response?.data || 'Erro ao carregar listas',
                color: 'error'
            }
        }
    },
    methods: {
        openLista(id) {
            this.$router.push({ name: 'listaEditar', params: { id } })
        },
        async addLista() {
            try {
                const novaLista = await addLista('Nova Lista')
                this.userStore.listas.push(novaLista)
                this.snackbar = {
                    show: true,
                    text: 'Lista criada com sucesso!',
                    color: 'success'
                }
                this.$router.push({ name: 'listaEditar', params: { id: novaLista.idLista } })
            } catch (err) {
                this.snackbar = {
                    show: true,
                    text: err.response?.data || 'Erro ao criar lista',
                    color: 'error'
                }
            }
        },
        async removeLista(id) {
            this.listaParaExcluir = id;
            this.snackbar = {
                show: true,
                text: 'Deseja excluir esta lista?',
                color: 'warning',
                timeout: -1,
                showAction: true,
                actionText: 'Excluir',
                actionColor: 'error',
                onAction: this.confirmarExclusao
            };
        },
        
        async confirmarExclusao() {
            try {
                const id = this.listaParaExcluir;
                await deleteLista(id);
                this.userStore.listas = this.userStore.listas.filter(l => l.idLista !== id);
                this.snackbar = {
                    show: true,
                    text: 'Lista removida com sucesso!',
                    color: 'success',
                    timeout: 4000,
                    showAction: false
                };
            } catch (err) {
                this.snackbar = {
                    show: true,
                    text: err.response?.data || 'Erro ao deletar lista',
                    color: 'error',
                    timeout: 4000,
                    showAction: false
                };
            } finally {
                this.listaParaExcluir = null;
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