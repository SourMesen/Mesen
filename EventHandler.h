#include "stdafx.h"

template<typename TypeA, typename TypeB>
class Tuple
{
	public:
		TypeA A;
		TypeB B;
		Tuple(TypeA A, TypeB B) : A(A), B(B) { }
};

template<typename ObjectType, typename... Types>
class EventHandler
{
	typedef void(ObjectType::*Func)(Types...);
	typedef Tuple<Func, ObjectType*> Handler;

	private: 
		std::list<Handler*> _handlerList;
		Handler *_singleHandler = nullptr;
		uint16_t _handlerCount = 0;

	public:
		void RegisterHandler(ObjectType *instance, Func handler) {
			this->_handlerList.push_back(new Handler(handler, instance));
			_singleHandler = this->_handlerList.size() == 1 ? this->_handlerList.front() : nullptr;
			_handlerCount++;
		}

		void CallHandler(Handler handler, Types... types) {
			Func callback = handler.A;
			ObjectType *instance = handler.B;
			(instance->*callback)(types...);
		}

		void operator+=(Func handler) {
			this->RegisterHandler(handler);
		}

		void operator()(Types... types) {
			if(_handlerCount > 0 && _singleHandler != nullptr) {
				Func callback = _singleHandler->A;
				ObjectType *instance = _singleHandler->B;
				(instance->*callback)(types...);
			} /*else {
					for(Handler handler : this->_handlerList) {
						Func callback = handler.A;
						ObjectType *instance = handler.B;
						(instance->*callback)(types...);
					}
				}
			}*/
		}
};