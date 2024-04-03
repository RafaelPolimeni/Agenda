<template>
    <div>
        <h2>{{ isNew ? 'Adicionar' : 'Editar' }} Contato</h2>
        <form @submit.prevent="saveContact">
            <input type="text" v-model="contact.nome" placeholder="Nome" required />
            <input type="email" v-model="contact.email" placeholder="E-mail" required />
            <input type="tel" v-model="contact.telefone" placeholder="Telefone" required />
            <button type="submit">{{ isNew ? 'Adicionar' : 'Salvar' }}</button>
        </form>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                contact: {
                    id: null,
                    nome: "",
                    email: "",
                    telefone: ""
                }
            };
        },
        props: {
            isNew: {
                type: Boolean,
                default: true
            },
            initialContact: {
                type: Object,
                default: null
            }
        },
        mounted() {
            if (!this.isNew && this.initialContact) {
                // Se não é um novo contato e foi passado um contato inicial, preenche o formulário com os dados do contato
                this.contact.id = this.initialContact.id;
                this.contact.nome = this.initialContact.nome;
                this.contact.email = this.initialContact.email;
                this.contact.telefone = this.initialContact.telefone;
            }
        },
        methods: {
            async saveContact() {
                try {
                    let response;
                    if (this.isNew) {
                        // Se é um novo contato, faz uma requisição POST para adicionar o contato
                        response = await fetch("http://localhost:5000/api/contacts", {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(this.contact)
                        });
                    } else {
                        // Se é um contato existente, faz uma requisição PUT para atualizá-lo
                        response = await fetch(`http://localhost:5000/api/contacts/${this.contact.id}`, {
                            method: "PUT",
                            headers: {
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(this.contact)
                        });
                    }

                    if (!response.ok) {
                        throw new Error(this.isNew ? "Erro ao adicionar contato" : "Erro ao atualizar contato");
                    }

                    // Limpa os campos do formulário após adicionar ou atualizar o contato
                    this.contact.id = null;
                    this.contact.nome = "";
                    this.contact.email = "";
                    this.contact.telefone = "";

                    // Emitir um evento para notificar que o contato foi adicionado ou atualizado
                    this.$emit(this.isNew ? "contact-added" : "contact-updated");
                } catch (error) {
                    console.error(error);
                }
            }
        }
    };
</script>
