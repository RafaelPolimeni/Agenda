<template>
    <div>
        <h2>Lista de Contatos</h2>
        <ul>
            <li v-for="contact in contacts" :key="contact.id">
                {{ contact.nome }} - {{ contact.email }} - {{ contact.telefone }}
                <button @click="editContact(contact)">Editar</button>
                <button @click="deleteContact(contact.id)">Excluir</button>
            </li>
        </ul>
        <!-- Modal para editar contato -->
        <div v-if="showModal" class="modal">
            <div class="modal-content">
                <span @click="closeModal" class="close">&times;</span>
                <h2>Editar Contato</h2>
                <form @submit.prevent="updateContact">
                    <input type="text" v-model="editedContact.nome" placeholder="Nome" required />
                    <input type="email" v-model="editedContact.email" placeholder="E-mail" required />
                    <input type="tel" v-model="editedContact.telefone" placeholder="Telefone" required />
                    <button type="submit">Salvar</button>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
  data() {
    return {
      contacts: [],
      editedContact: {
        id: null,
        nome: "",
        email: "",
        telefone: ""
      },
      showModal: false
    };
  },
        methods: {
            async loadContacts() {
                try {
                    const response = await fetch("http://localhost:5000/api/contacts");
                    if (!response.ok) {
                        throw new Error("Erro ao carregar contatos");
                    }

                    const data = await response.json();
                    this.contacts = data;

                } catch (error) {
                    console.error(error);
                }
            },

            editContact(contact) {
                // Copiar os dados do contato para editedContact
      this.editedContact.id = contact.id;
      this.editedContact.name = contact.name;
      this.editedContact.email = contact.email;
      this.editedContact.phone = contact.phone;

      // Exibir o modal de edição
      this.showModal = true;
    },
    async updateContact() {
      try {
        const response = await fetch(`http://localhost:5000/api/contacts/${this.editedContact.id}`, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(this.editedContact)
        });

        if (!response.ok) {
          throw new Error("Erro ao atualizar contato");
        }

        // Fechar o modal após a atualização bem-sucedida
        this.showModal = false;

        // Atualizar os dados do contato na lista localmente
        const index = this.contacts.findIndex(contact => contact.id === this.editedContact.id);
        if (index !== -1) {
          this.contacts.splice(index, 1, { ...this.editedContact });
        }
      } catch (error) {
        console.error(error);
      }
    },
    closeModal() {
      this.showModal = false;
    },
            },
            deleteContact(id) {
                const response = await fetch(`http://localhost:5000/api/contacts/${id}`, {
                    method: "DELETE"
                });

                if (!response.ok) {
                    throw new Error("Erro ao excluir contato");
                }

                // Remover o contato da lista localmente após a exclusão bem-sucedida
                this.contacts = this.contacts.filter(contact => contact.id !== id);
            } catch(error) {
                console.error(error);
            }
        }
    },
    mounted() {
        this.loadContacts();
    }
};
</script>
