<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <p>
      For a guide and recipes on how to configure / customize this project,<br>
      check out the
      <a href="https://cli.vuejs.org" target="_blank" rel="noopener">vue-cli documentation</a>.
    </p>
    <h3>Installed CLI Plugins</h3>
    <ul>
      <li><a href="https://github.com/vuejs/vue-cli/tree/dev/packages/%40vue/cli-plugin-babel" target="_blank" rel="noopener">babel</a></li>
      <li><a href="https://github.com/vuejs/vue-cli/tree/dev/packages/%40vue/cli-plugin-pwa" target="_blank" rel="noopener">pwa</a></li>
      <li><a href="https://github.com/vuejs/vue-cli/tree/dev/packages/%40vue/cli-plugin-eslint" target="_blank" rel="noopener">eslint</a></li>
    </ul>
    <h3>Essential Links</h3>
    <ul>
      <li><a href="https://vuejs.org" target="_blank" rel="noopener">Core Docs</a></li>
      <li><a href="https://forum.vuejs.org" target="_blank" rel="noopener">Forum</a></li>
      <li><a href="https://chat.vuejs.org" target="_blank" rel="noopener">Community Chat</a></li>
      <li><a href="https://twitter.com/vuejs" target="_blank" rel="noopener">Twitter</a></li>
      <li><a href="https://news.vuejs.org" target="_blank" rel="noopener">News</a></li>
    </ul>
    <h3>Ecosystem</h3>
    <ul>
      <li><a href="https://router.vuejs.org" target="_blank" rel="noopener">vue-router</a></li>
      <li><a href="https://vuex.vuejs.org" target="_blank" rel="noopener">vuex</a></li>
      <li><a href="https://github.com/vuejs/vue-devtools#vue-devtools" target="_blank" rel="noopener">vue-devtools</a></li>
      <li><a href="https://vue-loader.vuejs.org" target="_blank" rel="noopener">vue-loader</a></li>
      <li><a href="https://github.com/vuejs/awesome-vue" target="_blank" rel="noopener">awesome-vue</a></li>
    </ul>

    <DoubleRangeSlider :min="min" :max="max" @update:min="value => min = value" @update:max="value => max = value"></DoubleRangeSlider>

    <label>Min</label><input type="text" v-model="min">
    <label>Max</label><input type="text" v-model="max">
    <RatingComponent :value=5 :total=10></RatingComponent>
    <RatingComponent :value=3.5 :total=5></RatingComponent>

    <p v-for="idx in numParagraphs" v-bind:key="idx">
      Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ullamcorper erat erat, vel molestie mauris facilisis eget. Nunc sed risus vitae metus cursus suscipit nec nec libero. Suspendisse lacinia arcu lacus, eget semper nulla sollicitudin id. Suspendisse ac lobortis ligula. Nunc dignissim ex id est mattis, quis egestas risus aliquet. Curabitur lorem tortor, feugiat et nisl congue, finibus finibus libero. Maecenas euismod velit a gravida bibendum. Maecenas faucibus felis vitae dui finibus, ornare gravida turpis suscipit.
    </p>
    <ScrollTopArrow></ScrollTopArrow>

  </div>
</template>

<script>

import RatingComponent from '@/components/shared/rating/stars'
import ScrollTopArrow from '@/components/shared/blog/ScrollTopArrow'
import DoubleRangeSlider from '@/components/shared/slider/DoubleRangeSlider'

export default {
  data () {
    return {
      numParagraphs: 10,
      min: 0,
      max: 10
    }
  },
  components: {
    ScrollTopArrow,
    RatingComponent,
    DoubleRangeSlider
  },
  name: 'HelloWorld',
  props: {
    msg: String
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="less">
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
