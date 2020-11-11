---
title: JSON to python object
published: true
description: Different approaches to mapping json to python objects and vice versa
tags: python, json, encoding, decoding
cover_image: https://pythontic.com/json_encoding_python.png
---

This is a very fast how-to encode and decode python objects to json.

* For decoding we will use the parameter object_hook. With it, we can pass a function to the json.loads function.
* For encoding we will use a Custom JSON encoder.

### Mapping incoming object with standard library

```python
import json
my_json_str = '{"name":"John", "age":31, "city":"New York"}'
my_dict_json = json.loads(my_json_str)
print(my_json_str)
# output: {"name":"John", "age":31, "city":"New York"}
```

Now, with a dictionary we have different approaches for making the object mapping

### Mapping approach with dict and constructor´s parameters

```python
class MyObject(object):
    def __init__(self, age:int, city:str, name:str):
        self.name = name
        self.age = age
        self.city = city

    def __repr__(self):
        return f"{self.name} {self.age} {self.city}"

my_obj = json.loads(my_json_str, object_hook=lambda data: MyObject(**data))

print(my_obj)

# output: John 31 New York
```

### Decode class with a Custom encoder

```python
class CustomJSONEncoder(json.JSONEncoder):
    def default(self, obj):
        if issubclass(type(obj), MyObject):
            return json.dumps(obj.__dict__, default=lambda _: None)
        return json.JSONEncoder.default(self, obj)     

obj_str = json.dumps(my_obj, default=CustomJSONEncoder().default)
print(obj_str)
#output: "{\"name\": \"John\", \"age\": 31, \"city\": \"New York\"}"
```

### Mapping approach with NamedTuple

```python
from typing import NamedTuple

class MyObjectTuple(NamedTuple):
    name:str
    city:str
    age:int

my_obj = json.loads(my_json_str, object_hook=lambda data: MyObjectTuple(**data))

print(my_obj)
# output: MyObjectTuple(name='John', city='New York', age=31)
print((my_obj)._asdict())
# output: {'name': 'John', 'city': 'New York', 'age': 31}
```
### Decode class without custom encoder

```python
obj_str = json.dumps(my_obj._asdict())
print(obj_str)
# output: {"name": "John", "city": "New York", "age": 31}
```

Do you any other method to object decode/encode? Do you think that it can be done better?. Leave your thoughts below, feedback is always welcomed :)