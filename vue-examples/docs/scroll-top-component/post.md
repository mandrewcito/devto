---
title: VueJS scroll top component 
published: true
description: Creating a scroll top component from scratch
tags: vue, scroll, component, slot
series: vue examples
cover_image: https://i.blogs.es/99f0c0/vue/450_1000.png
---

To start my weekend i have decided code a customizable scroll top component, component will be implemented with vue slots. Slots will allow us to passing any kind of html element to the component.

### Component implementation

We will bind to scroll event and check Y axis scroll, this will allow us to hide/show compoment depending on the scroll of the page.

Next, we will make a function that scroll to top of the page step by step and make an simple animation.

#### ScrollTopComponent

```vue
<template>
    <a @click="scrollTop" v-show="visible" class="bottom-right">
        <slot></slot>
    </a>
</template>

<script>
export default {
  data () {
    return {
      visible: false
    }
  },
  methods: {
    scrollTop: function () {
      this.intervalId = setInterval(() => {
        if (window.pageYOffset === 0) {
          clearInterval(this.intervalId)
        }
        window.scroll(0, window.pageYOffset - 50)
      }, 20)
    },
    scrollListener: function (e) {
      this.visible = window.scrollY > 150
    }
  },
  mounted: function () {
    window.addEventListener('scroll', this.scrollListener)
  },
  beforeDestroy: function () {
    window.removeEventListener('scroll', this.scrollListener)
  }
}
</script>

<style scoped>
.bottom-right {
  position: fixed;
  bottom: 20px;
  right: 20px;
  cursor: pointer;
}
</style>

```

#### ScrollTopArrow

Now, with a generic component we will implement a new one: we will use font awesome icon and  a simple css styles.

```vue
<template>
  <ScrollTopComponent>
      <a class="btn btn-light">
        <font-awesome-icon :icon="['fas', 'angle-double-up']" size="3x" />
      </a>
  </ScrollTopComponent>
</template>

<script>
import ScrollTopComponent from './ScrollTopComponent'
export default {
  components: {
    ScrollTopComponent
  }
}
</script>

<style>
.btn {
    border-radius: 8px;
    background-color: rgba(0, 0, 0, 0.55);
    padding-top: 27px;
    padding-left: 10px;
    padding-right: 10px;
    padding-bottom: 5px;
}
</style>

```


### Use

The component using is quite simple, we only need to place component on the DOM.

```vue
<template>
    <div class="container">
        <p>A super long component</p>
        <ScrollTopArrow></ScrollTopArrow>
    </div>
</template>
<script>
import ScrollTopArrow from '@/components/shared/blog/ScrollTopArrow'
export default {
  components: {
    ScrollTopArrow
  }
}
</script>
```

### Result

Now when we are on top of the page component is hidden but when we scroll component shows up, you can see component ´s implementation on my github.

![Example gif](https://media.giphy.com/media/Icp0uDdl7Qxe7bHFM5/giphy.gif)

## References

[Github](https://github.com/mandrewcito/devto/tree/master/vue-examples)
[vue font awesome](https://github.com/FortAwesome/vue-fontawesome)