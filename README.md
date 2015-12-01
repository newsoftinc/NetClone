# NetClone
A .NET Library for object cloning

This library uses a fluent helper to configure object clone configuration. You can clone objects or configure members where references needs to be preserved.

Library has been production tested and has no currently known issues. 

Example :
```csharp
var clone = new ObjectCloner<A>();
clone.ForMember(V => V.B).CloneMode(CloneMode.Copy);
clone.ForMember(V => V.B.C).CloneMode(CloneMode.Reference);
```
It has support for collections
```csharp
var clone = new ObjectCloner<A>();
clone.ForMember(V => V.Bs.Select(V1=>V1.B).CloneMode(CloneMode.Copy);
```

It uses Newtonsoft JSON Serializer to copy the objects. 

Library also has capability to configure clone on non-generic fluent configuration.

Clone a collection member
```csharp
            var clone = new ObjectCloner();
            var a = Helper.InitTestObject();
            clone.ForMember("Bs[*].C").CloneMode(CloneMode.Copy);
```

Set member of a collection to preserve reference
```csharp
            var clone = new ObjectCloner();
            clone.ForMember("Bs[*]").CloneMode(CloneMode.Copy);
            clone.ForMember("Bs[*].C").CloneMode(CloneMode.AsReference);
```
