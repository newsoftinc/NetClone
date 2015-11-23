# NetClone
A .NET Library for object cloning

This library uses a fluent helper to configure object clone configuration. You can clone objects or assign reference of given members.

Example :

var clone = new ObjectCloner<A>();
clone.ForMember(V => V.B).CloneMode(CloneMode.Copy);
clone.ForMember(V => V.B.C).CloneMode(CloneMode.Reference);

It has support for collections

var clone = new ObjectCloner<A>();
clone.ForMember(V => V.Bs.Select(V1=>V1.B).CloneMode(CloneMode.Copy);


It uses Newtonsoft JSON Serializer to copy the objects. 
