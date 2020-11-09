<template>
  <v-form v-model="isValid">
    <h1 style="color: white">
      Añade tu información
    </h1>
    <v-row>
      <v-col
        cols="12"
        md="6"
      >
        <v-text-field
          v-model="order.client.name"
          placeholder="Nombre"
          dark
          filled
          required
        />
      </v-col>
      <v-col
        cols="12"
        md="6"
      >
        <v-text-field
          v-model="order.client.surname"
          placeholder="Apellido"
          dark
          filled
          required
        />
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="12"
        md="6"
      >
        <v-text-field
          v-model="order.client.phoneNumber"
          placeholder="Número de teléfono"
          dark
          filled
        />
      </v-col>
      <v-col
        cols="12"
        md="6"
      >
        <v-text-field
          v-model="order.client.email"
          placeholder="Correo electrónico"
          dark
          filled
          required
        />
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="12"
      >
        <v-textarea
          v-model="order.additionalInformation"
          aria-label="Información adicional del pedido"
          placeholder="Añade informarción extra del pedido"
          dark
          filled
          required
        />
      </v-col>
    </v-row>
    <v-row class="content-flex-end">
      <v-col
        cols="12"
        md="4"
      >
        <v-btn
          aria-label="Solicitar compra"
          @click.prevent="requestPurchase"
        >
          Solicitar compra
        </v-btn>
      </v-col>
    </v-row>
  </v-form>
</template>

<script lang="ts">
import Vue from 'vue'
import { Client } from '../services/purchase-aplication/models/purchase-aplication.models'

export default Vue.extend({
  name: 'OrderForm',
  data () {
    return {
      isValid: true,
      order: {
        client: {
          name: '',
          email: ''
        } as Client,
        additionalInformation: ''
      }
    }
  },
  methods: {
    requestPurchase () {
      if (this.isValid) {
        this.$emit('onRequest', this.order)
        this.order = {
          client: {
            name: '',
            email: ''
          },
          additionalInformation: ''
        }
      }
    }
  }
})
</script>

<style scoped>
  button {
    width: inherit;
  }

  .content-flex-end {
    justify-content: flex-end;
  }
</style>
