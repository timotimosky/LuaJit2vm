#pragma once

#include "ClassA.h"

#include <iostream>

#include <mono/jit/jit.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/class.h>
#include <mono/metadata/debug-helpers.h>
#include <mono/metadata/mono-config.h>

using namespace std;

MonoDomain* domain;

struct DataOnHead
{
    void* _cppObjectPtr;
};

static const int MonoObjectMemoryOffset = sizeof(void*) * 2;

DataOnHead* getDataOnHead(MonoObject* monoObject)
{
    return reinterpret_cast<DataOnHead*> (((char*)monoObject) + MonoObjectMemoryOffset);
}

void bindCppToMono(void* cppPtr, MonoObject* monoObject)
{
    getDataOnHead(monoObject)->_cppObjectPtr = cppPtr;
}

template<class T>
void bindMonoToCpp(MonoObject* monoObject, T* cppPtr)
{
    cppPtr->setMonoObject(monoObject);
}

template<class T>
void bindCppAndMono(MonoObject* monoObject, T* cppPtr)
{
    bindCppToMono(cppPtr, monoObject);
    bindMonoToCpp(monoObject, cppPtr);
}

template<class T>
T* getCppFromMono(MonoObject* monoObject)
{
    return static_cast<T*>(getDataOnHead(monoObject)->_cppObjectPtr);
}

template<class T>
MonoArray* VectorToMonoArray(std::vector<T*> vec, MonoClass* monoClass)
{
    MonoArray* pMonoArr = NULL;
    //MonoClass* pMonoClass = mono_object_get_class(T::getTypeNameStatic());
    MonoDomain* pDomain = mono_domain_get();
    MonoType* t = mono_class_get_type(monoClass);
    size_t size = vec.size();

    // - create a ubyte array
    pMonoArr = mono_array_new(pDomain, monoClass, size);
    if (NULL == pMonoArr)
    {
        return NULL;
    }
    // - convert string into mono string and add them to a mono byte array
    for (uint32 ii = 0; ii < size; ii++)
    {
        MonoObject* tp = vec[ii]->getMonoObject();

        mono_array_set(pMonoArr, MonoObject*, ii, tp);
    }

    return pMonoArr;
}

template<class T>
std::vector<T*> MonoArrayToVector(MonoArray* monoArray)
{

    MonoObject* pStr = NULL;
    size_t size = mono_array_length(monoArray);
    std::vector<T*> arr;
    for (uint32 ii = 0; ii < size; ii++)
    {
        pStr = mono_array_get(monoArray, MonoObject*, ii);
        T* tmp = convert<T>(pStr);
        arr.push_back(tmp);
    }

    return arr;
}


template<class T>
static T* convert(MonoObject* self)
{
    T* native_handle = getCppFromMono<T>(self);

    //MonoClass* monoClass;

    //monoClass = mono_object_get_class(self);

    //monoClas = mono_class_from_name(image, "TestAllRuntime", "ClassA");

    //MonoClassField* field;
    //field = mono_class_get_field_from_name(monoClass, "native_handle");

    //mono_field_get_value(self, field, (void*)&native_handle);

    return native_handle;
}

template<class T>
static void Class_bind(MonoObject* self)
{
    T* native_handle = new T();

    //MonoClass* monoClass;

    //monoClass=mono_object_get_class(self);

    //monoClas = mono_class_from_name(image, "TestAllRuntime", "ClassA");

    //MonoClassField* field;
    //field = mono_class_get_field_from_name(monoClass, "native_handle");

    //mono_field_set_value(self, field, (void*)&native_handle);
    //T* handle=getCppFromMono<T>(self);

    bindCppAndMono<T>(self, native_handle);

    //handle = native_handle;

    //native_handle->setMonoObject(self);
    //bindCppAndMono(self,t);
}

static void ClassA_bind(MonoObject* self)
{
    Class_bind<ClassA>(self);
}

static void ClassA_setInt(MonoObject* self, int intValue)
{
    //ClassA* a = getCppFromMono<ClassA>(self);

    ClassA* a = convert<ClassA>(self);

    a->setInt(intValue);

    //a->setInt(intValue);
}

static int ClassA_getInt(MonoObject* self)
{
    //ClassA* a = getCppFromMono<ClassA>(self);

    ClassA* a = convert<ClassA>(self);

    return a->getInt();
    //return a->getInt();
}

MonoImage* image;

static void ClassA_setFloat(MonoObject* self, float floatValue)
{
    //ClassA* a = getCppFromMono<ClassA>(self);

    ClassA* a = convert<ClassA>(self);

    a->setFloat(floatValue);

    //self->setFloat(floatValue);
}

static float ClassA_getFloat(MonoObject* self)
{
    //ClassA* a = getCppFromMono<ClassA>(self);

    ClassA* a = convert<ClassA>(self);

    return a->getFloat();

    //return self->getFloat();
}

static void ClassA_setString(MonoObject* self, MonoString* stringValue)
{
    //ClassA* a = getCppFromMono<ClassA>(self);

    ClassA* a = convert<ClassA>(self);

    char* pStr = mono_string_to_utf8(stringValue);
    std::string str = pStr;
    mono_free(pStr);
    //std::cout << str<<std::endl;

    //str = "test gulugulu";
    //std::string aaa = "tset aaaaab";
    a->setString(str);
}

static MonoString* ClassA_getString(MonoObject* self)
{
    //ClassA* a = getCppFromMono<ClassA>(self);

    ClassA* a = convert<ClassA>(self);

    std::string s = a->getString();

    //s = "test hahahahahaha";
    //cout <<"haha  "<< s << endl;
    //mono_string_new_wrapper
    MonoString* str = mono_string_new(domain, s.c_str());
    //MonoString* str = mono_string_new_wrapper(s.c_str());
    //MonoString* str = mono_string_new(domain, s.c_str()); 
    //mono_string_new_wrapper
    return str;
}

static void ClassA_setDirection(MonoObject* self, Direction dir)
{
    ClassA* t = convert<ClassA>(self);

    t->setDirection(dir);

}

static void ClassA_setData(MonoObject* self, MonoArray* data)
{
    std::vector<ClassB*> vec = MonoArrayToVector<ClassB>(data);

    ClassA* tmp = convert<ClassA>(self);

    tmp->setData(vec);
}

static MonoArray* ClassA_getData(MonoObject* self)
{
    ClassA* tmp = convert<ClassA>(self);

    std::vector<ClassB*> vec = tmp->getData();

    MonoClass* cl = mono_class_from_name(image, "TestAllRuntime", "ClassB");

    return VectorToMonoArray<ClassB>(vec, cl);
}

static void ClassA_setCallback(MonoObject* self, MonoObject* object, MonoString* funcName)
{
    MonoClass* c = mono_object_get_class(self);
    MonoClass* b = mono_object_get_class(object);

    char* pStr = mono_string_to_utf8(funcName);
    std::string str = pStr;
    mono_free(pStr);

    MonoMethod* m = mono_class_get_method_from_name(b, str.c_str(), 0);

    mono_runtime_invoke(m, self, NULL, NULL);
}

void ClassB_bind(MonoObject* self)
{
    Class_bind<ClassB>(self);
}

int ClassB_getValue(MonoObject* self)
{
    ClassB* tmp = convert<ClassB>(self);

    return tmp->getValue();
}

void ClassB_setValue(MonoObject* self, int value)
{
    ClassB* tmp = convert<ClassB>(self);

    tmp->setValue(value);
}

void ClassC_bind(MonoObject* self)
{
    Class_bind<ClassC>(self);
}

float ClassC_getFloat(MonoObject* self)
{
    ClassC* tmp = convert<ClassC>(self);

    return tmp->getFloat();
}

void ClassC_setFloat(MonoObject* self, float value)
{
    ClassC* tmp = convert<ClassC>(self);

    tmp->setFloat(value);
}

void ClassD_bind(MonoObject* self)
{
    Class_bind<ClassD>(self);
}

float ClassD_getFF(MonoObject* self)
{
    ClassD* tmp = convert<ClassD>(self);

    return tmp->getFF();
}

void ClassD_setFF(MonoObject* self, float ff)
{
    ClassD* tmp = convert<ClassD>(self);

    tmp->setFF(ff);
}

void main()
{
    // ��������·������Ҫ��������mscorlib.dll
    std::string MonoPath = "C:/Program Files (x86)/Mono";
    std::string ManagedLibraryPath = "E:/vsPrjs/Dinky2D/TestAllRuntime/TestAllRuntime.dll";

    std::string monoPath = MonoPath;
    std::string monoLibPath = monoPath + "/lib";
    std::string monoEtcPath = monoPath + "/etc";

    mono_set_dirs(monoLibPath.c_str(),
        monoEtcPath.c_str());
    // Program.cs������dll���ڵ�λ��
    const char* managed_binary_path = ManagedLibraryPath.c_str();

    //��ȡӦ����
    domain = mono_jit_init("TestAllRuntime");
    mono_config_parse(NULL);

    //���س���ManagedLibrary.dll
    MonoAssembly* assembly = mono_domain_assembly_open(domain, managed_binary_path);
    image = mono_assembly_get_image(assembly);

    //====================================================================
    // -----------------------

    // -- Base - Texture
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_bind",
        reinterpret_cast<void*>(&ClassA_bind));
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_setInt", reinterpret_cast<void*>(&ClassA_setInt));
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_getInt", reinterpret_cast<void*>(&ClassA_getInt));
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_setFloat", reinterpret_cast<void*>(&ClassA_setFloat));
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_getFloat", reinterpret_cast<void*>(&ClassA_getFloat));
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_setString", reinterpret_cast<void*>(&ClassA_setString));
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_getString", reinterpret_cast<void*>(&ClassA_getString));
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_setDirection", reinterpret_cast<void*>(&ClassA_setDirection));
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_setData",
        reinterpret_cast<void*>(&ClassA_setData)
    );
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_getData",
        reinterpret_cast<void*>(&ClassA_getData)
    );
    mono_add_internal_call("TestAllRuntime.ClassA::ClassA_setCallback",
        reinterpret_cast<void*>(&ClassA_setCallback)
    );

    mono_add_internal_call("TestAllRuntime.ClassB::ClassB_bind", reinterpret_cast<void*>(&ClassB_bind));
    mono_add_internal_call("TestAllRuntime.ClassB::ClassB_getValue", reinterpret_cast<void*>(&ClassB_getValue));
    mono_add_internal_call("TestAllRuntime.ClassB::ClassB_setValue", reinterpret_cast<void*>(&ClassB_setValue));
    mono_add_internal_call("TestAllRuntime.ClassC::ClassC_bind", reinterpret_cast<void*>(&ClassC_bind));
    mono_add_internal_call("TestAllRuntime.ClassC::ClassC_getFloat", reinterpret_cast<void*>(&ClassC_getFloat));
    mono_add_internal_call("TestAllRuntime.ClassC::ClassC_setFloat", reinterpret_cast<void*>(&ClassC_setFloat));
    mono_add_internal_call("TestAllRuntime.ClassD::ClassD_bind", reinterpret_cast<void*>(&ClassD_bind));
    mono_add_internal_call("TestAllRuntime.ClassD::ClassD_getFF", reinterpret_cast<void*>(&ClassD_getFF));
    mono_add_internal_call("TestAllRuntime.ClassD::ClassD_setFF", reinterpret_cast<void*>(&ClassD_setFF));
    //===============================================================================
    MonoClass* main_class = mono_class_from_name(image, "TestAllRuntime", "Main");

    const bool include_namespace = true;
    MonoMethodDesc* managed_method_desc = mono_method_desc_new("TestAllRuntime.Main:main()", include_namespace);
    MonoMethod* managed_method = mono_method_desc_search_in_class(managed_method_desc, main_class);
    mono_method_desc_free(managed_method_desc);

    mono_runtime_invoke(managed_method, NULL, NULL, NULL);

    mono_jit_cleanup(domain);
}
