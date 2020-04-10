---
title: Vue rating component 
published: true
description: Creating a rating component with stars from scratch
tags: vue, rating, component, slot
series: vue examples
cover_image: https://i.blogs.es/99f0c0/vue/450_1000.png
---

Today i will implement amazon rating component, i will build it with vue and font awesome. but it can be done with any library with icons with stars filled, half filled and empty.


![Alt Text](https://dev-to-uploads.s3.amazonaws.com/i/sms08qe1zbab2af5kkbl.PNG)

### Component implementation

Componnent will be very simple, we call it with 2 params. Number total of points(max stars) and the score(value of stars). With this parameters our component will draw the number of star filled, partialy filled and empty.

Previously to the implementation, we will need passing a number to a array of numbers in javascript. There are a lot of implementations i only show 2. There are a lot of posts talking about [this](https://www.freecodecamp.org/news/https-medium-com-gladchinda-hacks-for-creating-javascript-arrays-a1b80cb372b/).

#### Making an array from a number

I´m currently using the following function

```javascript
const range = (start, end, length = end - start + 1) =>
  Array.from({ length }, (_, i) => start + i)

range(0, 5)
// [0, 1, 2, 3, 4]
```
but the following function can be used too:

```javascript
[...Array(5).keys()]
// [0, 1, 2, 3, 4]
```
both functions will create an of number of a specified lenght.

#### Component

Now i will code component with font awesome components, but on the next section i will decouple component from library.

```vue
<template>
  <div class="rating-container">
      <font-awesome-icon icon="star"  v-for="idx in completeRange" v-bind:key="idx + 'st'" />
      <font-awesome-icon :icon="['fas', 'star-half-alt']"  v-for="idx in halfRange" v-bind:key="idx + 'stt'" />
      <font-awesome-icon :icon="['far', 'star']"  v-for="idx in totalRange" v-bind:key="idx" />
  </div>
</template>

<script>
const range = (start, end, length = end - start + 1) =>
  Array.from({ length }, (_, i) => start + i)

export default {
  name: 'RatingComponent',
  props: {
    value: Number,
    total: Number
  },
  data () {
    return {
      completeRange: range(0, this.value - 1),
      halfRange: range(0, this.value % 1 === 0 ? 0 : 1),
      totalRange: range(0, this.total - this.value)
    }
  },
  created: function () {
    if (this.value > this.total) {
      throw new Error('total lower than value')
    }
  }
}
</script>

<style scoped>
.rating-container {
  display:flex;
  justify-content: center;
}
</style>

```

#### How to use 
```vue
<template>
    <RatingComponent :value=5 :total=10></RatingComponent>
    <RatingComponent :value=3.5 :total=5></RatingComponent>
</template>
<script>
import RatingComponent from '@/components/shared/rating/stars'
export default {
  components: {
    RatingComponent
  }
}
</script>
```

### Refactoring component, decoupling from font awesome

On the refactoring we will face to a big problem, we cant iterate slots. we need wrap it with another element
```vue
```
#### Component

```vue
<template>
  <div class="rating-container">
      <slot name="filled" v-bind="!!idx|| null" v-for="idx in completeRange" />
      <slot name="half" v-bind="!!idx|| null"  v-for="idx in halfRange" />
      <slot name="empty" v-bind="!!idx|| null"  v-for="idx in totalRange" />
  </div>
</template>

<script>
const range = (start, end, length = end - start + 1) =>
  Array.from({ length }, (_, i) => start + i)

export default {
  name: 'RatingDecoupledComponent',
  props: {
    value: Number,
    total: Number
  },
  data () {
    return {
      completeRange: range(0, this.value - 1),
      halfRange: range(0, this.value % 1 === 0 ? 0 : 1),
      totalRange: range(0, this.total - this.value)
    }
  },
  created: function () {
    if (this.value > this.total) {
      throw new Error('total lower than value')
    }
  }
}
</script>
```

#### How to use 

```vue
<template>
    <RatingDecoupledComponent :value=3.5 :total=5>
        <font-awesome-icon  slot="filled" icon="star" />
        <font-awesome-icon slot="half" :icon="['fas', 'star-half-alt']" />
        <font-awesome-icon  slot="empty" :icon="['far', 'star']" />
    </RatingDecoupledComponent>
</template>

<script>
import RatingDecoupledComponent from '@/components/shared/rating/StarsDecoupled'
export default {
  components: {
    RatingDecoupledComponent
  }
}
</script>
```

## Result

Example component will look like this:

![Example component demo](https://dev-to-uploads.s3.amazonaws.com/i/44wftvoqosaknwda58xy.PNG)

Despite of decoupling components from libraries is a good choice, changing icons library is something that i dint't do ofendly. But this is a  implementation that i did  only for fun .With the  porpuse of taking a look to implementation, and check if it is usable. But in my opinion doesnt make sense on a application. I prefer simpler components.

## References

[Github](https://github.com/mandrewcito/devto/tree/master/vue-examples)
[vue font awesome](https://github.com/FortAwesome/vue-fontawesome)