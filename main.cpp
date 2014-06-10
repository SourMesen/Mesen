#include "stdafx.h"
#include "CPU.h"

/*
template<typename... Types>
class Event
{
	typedef void(*Func)(Types...);
	private: 
		std::list<Func> handlerList;
		Func lastFunc;

	public:
		void RegisterHandler(Func handler);
		void operator+=(Func handler);
		void operator()(Types ...types);
};

template<typename... Types>
void Event<Types...>::RegisterHandler(Func handler)
{
	this->handlerList.push_back(handler);
	lastFunc = this->handlerList.size() == 1 ? handler : nullptr;
}

template<typename... Types>
void Event<Types...>::operator+=(Func handler) {
	this->RegisterHandler(handler);
}

template<typename... Types>
void Event<Types...>::operator()(Types... types)
{
	if (lastFunc) {
		lastFunc(types...);
	} else {
		for (auto handler : this->handlerList) {
			handler(types...);
		}
	}
}

template<typename T>
class TemplatedClass
{
	public:
		T Sum(T a, T b);
};

template<typename T>
T TemplatedClass<T>::Sum(T a, T b)
{
	return a + b;
}

template<typename T> using ptr = std::unique_ptr<T>;

class Test
{
	private:
		int counter = 0;
		Test();

	public:
		int Sum(int, int);
		void NewThread();
		static std::unique_ptr<Test> NewInstance();
};

Test::Test()
{

}

int Test::Sum(int x, int y)
{
	return x + y;
}

void Test::NewThread()
{
	for (int i = 0; i < 100; i++) {
		std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(1000));
		printf("test %i", this->counter);
		this->counter++;
	}
}

std::unique_ptr<Test> Test::NewInstance()
{
	return std::unique_ptr<Test>(new Test());
}

void somefunction(void)
{
	printf("test11111");
}

void func2(int a)
{
	//printf("%i", a);
}*/

int _tmain(int argc, _TCHAR* argv[])
{
	std::ifstream romFile("6502_functional_test.bin", std::ios::in | std::ios::binary);
	if(!romFile) {
		std::cout << "Error";
	}

	int8_t romMemory[65536];
	romFile.read((char *)romMemory, 65536);
	/*int8_t* memory = new int8_t[8000];
	memory[0x0600] = 0xA9;
	memory[0x0601] = 0x55;*/
	
	Core core(romMemory);
	core.Exec();
		
	/*Event<> eventHandler;
	eventHandler += somefunction;

	Event<int> intHandler;
	intHandler += func2;
	intHandler += func2;
	intHandler += func2;
	
	for (int i = 0; i < 1000000; i++) {
		intHandler(i);
	}
	eventHandler();

	TemplatedClass<double> DoubleSum;
	printf("%d", DoubleSum.Sum(10.0, 20.0));
	
	TemplatedClass<std::string> StringSum;
	std::cout << StringSum.Sum("asdas", "dsadasdsa");

	auto str1 = std::string("aaaa");
	auto str2 = std::string("bbbb");
	auto str3 = str1 + str2;
	std::cout << str3;
	
	std::unique_ptr<Test> test;

	if (!test) {
		test = Test::NewInstance();
	}
	
	std::thread t1(&Test::NewThread, test.get());

	std::thread t2([]() {
		printf("test2");
	});

	if (test) {
		printf("%i", test->Sum(1000, 10));
	}

	t1.join();
	*/
	return 0;
}

