<script  lang="ts">
import { defineComponent } from 'vue';
import axios from "axios";
import { useUserStore } from '../stores/counter'

export default defineComponent({
  watch: {
  },
  data() {
    return {
      username: "",
      password: "",
      userId: "",
      loggedIn: false,
      rolled: 0,
      matchList: null
    }
  },

  // `mounted` is a lifecycle hook which we will explain later
  mounted() {
    this.getMatchList();
  },
  methods: {
    login() {
      const options = {
        method: `post`,
        url: `https://localhost:44348/api/login`,
        data: {
          username: this.username,
          password: this.password,
        },
        transformRequest: [(data, headers) => {
          return data;
        }]
      };
      const store = useUserStore()
      axios.post(`https://localhost:44348/api/login`, { username: this.username, password: this.password })
        .then(response => {
          store.token = response.data.token;
          this.loggedIn = true;
          this.userId = response.data.userId;
          console.log(response.data)
          alert("login successfull")
        }).catch(x => {
          alert("Username or password incorrect")
        });
    },
    logout() {
      const store = useUserStore()
      store.token = null;
      this.userId = 0;
      this.loggedIn = false;
    },
    roll() {
      const store = useUserStore()
      let user: string = this.userId;
      axios.post(`https://localhost:44348/api/play`, { userId: user }, {
        headers: {
          'authorization': `Bearer ${store.token}`,
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
        .then(response => {
          this.rolled = response.data;
        }).catch(x => {
          this.rolled="You have already rolled"
          this.getMatchList();
        });
    },
    refreshResults() {
      const store = useUserStore()
      let user: string = this.userId;
      axios.get(`https://localhost:44348/api/play/${user}`, {
        headers: {
          'authorization': `Bearer ${store.token}`,
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
        .then(response => {
          this.rolled = response.data;
        }).catch(x => {
          this.rolled="You havent rolled yet !Join the game!"
          this.getMatchList();
        });
    },
    getMatchList() {
      axios.get(`https://localhost:44348/api/match`)
        .then(response => {
          this.matchList = response.data;
          console.log(response.data)
        }).catch(x => {
        });
    }
  }


});
</script>

<template>
  <div v-if="!this.loggedIn">
    <input v-model="username" type="text" placeholder="userName" />
    <input v-model="password" type="password" placeholder="password" />
    <button @click="login()">Login in</button>
  </div>
  <div v-else>
    <button @click="logout()">logout</button>
    <div>
      <button @click="roll()">Play Game</button>
      <button @click="refreshResults()">Refresh for results</button>
      <h1>Your Roll is:</h1>
      <h1>{{ this.rolled }}</h1>
    </div>
  </div>
  <div>
    <li v-for="item in this.matchList">
    Name: {{ item.name }} -
    isComplated: {{item.isComplated}} -
    StartedAt: {{item.startedAt}} -
    ExpiresAt: {{item.expiresAt}} -
    WinnerRoll: {{item.winnerRoll}} -
    WinnerName: {{item.winnerName}} -
    </li>
  </div>
</template>

<style scoped>
</style>
