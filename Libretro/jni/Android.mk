LOCAL_PATH := $(call my-dir)

LIBRETRO_DIR := ../
SEVENZIP_DIR := ../../SevenZip
LUA_DIR      := ../../Lua
CORE_DIR     := ../../Core
UTIL_DIR     := ../../Utilities

INCFLAGS     :=
SOURCES_C    :=
SOURCES_CXX  :=

include $(CLEAR_VARS)

GIT_VERSION ?= " $(shell git rev-parse --short HEAD || echo unknown)"
ifneq ($(GIT_VERSION)," unknown")
   LOCAL_CXXFLAGS += -DGIT_VERSION=\"$(GIT_VERSION)\"
endif

HAVE_NETWORK = 1
LOCAL_MODULE    := libretro

ifeq ($(TARGET_ARCH),arm)
LOCAL_CXXFLAGS += -DANDROID_ARM
LOCAL_ARM_MODE := arm
endif

ifeq ($(TARGET_ARCH),x86)
LOCAL_CXXFLAGS +=  -DANDROID_X86
endif

ifeq ($(TARGET_ARCH),mips)
LOCAL_CXXFLAGS += -DANDROID_MIPS
endif

include ../Makefile.common

COREFLAGS := -DINLINE=inline -DHAVE_STDINT_H -DHAVE_INTTYPES_H -DLIBRETRO -DNDEBUG -D_USE_MATH_DEFINES -I$(CORE_DIR) -DDISABLE_DEBUGGER -DDISABLE_TIMEKEEPING -Wno-multichar $(INCFLAGS)

LOCAL_SRC_FILES := $(SOURCES_CXX) $(SOURCES_C)
LOCAL_CFLAGS := $(COREFLAGS)
LOCAL_CXXFLAGS := $(COREFLAGS) -std=c++11
LOCAL_CPP_FEATURES := exceptions rtti
LOCAL_LDLIBS += -latomic

include $(BUILD_SHARED_LIBRARY)

