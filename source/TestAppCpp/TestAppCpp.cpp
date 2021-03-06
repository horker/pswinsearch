#include <Windows.h>
#include <stdio.h>
#include <propsys.h>

extern "C" HRESULT GetAllProperties(PWSTR* properties);

int main()
{
    CoInitialize(NULL);

    PWSTR p;
    auto hr = GetAllProperties(&p);
    if (FAILED(hr))
        return 1;
    wprintf(L"%s", p);

    return 0;
}
