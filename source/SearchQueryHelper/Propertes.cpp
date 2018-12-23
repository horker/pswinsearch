#include <vector>
#include <string>
#include <propsys.h>

// Example:
// github.com/Microsoft/Windows-classic-samples/Samples/Win7Samples/winui/shell/appplatform/propertyschemas/PropSchema.cpp

extern "C" __declspec(dllexport)
HRESULT GetAllProperties(PWSTR* properties)
{
    IPropertyDescriptionList* ppdl = 0;
    IPropertyDescription* ppd = 0;;
    HRESULT hr;
    UINT cuProps = 0;
    std::wstring buffer;

    hr = PSEnumeratePropertyDescriptions(PDEF_ALL, IID_PPV_ARGS(&ppdl));
    if (!SUCCEEDED(hr))
        goto cleanup;

    hr = ppdl->GetCount(&cuProps);
    if (!SUCCEEDED(hr))
        goto cleanup;

    PWSTR pszCanonicalName;
    PWSTR pszDisplayName;

    for (UINT i = 0; i < cuProps; ++i)
    {
        if (i > 0)
            buffer.append(L"\t");

        hr = ppdl->GetAt(i, IID_PPV_ARGS(&ppd));
        if (!SUCCEEDED(hr))
            goto cleanup;

        hr = ppd->GetCanonicalName(&pszCanonicalName);
        if (!SUCCEEDED(hr))
            goto cleanup;

        buffer.append(pszCanonicalName);
        buffer.append(L"\t");
        CoTaskMemFree(pszCanonicalName);

        hr = ppd->GetDisplayName(&pszDisplayName);
        if (SUCCEEDED(hr))
        {
            buffer.append(pszDisplayName);
            CoTaskMemFree(pszDisplayName);
        }
    }

    {
        wchar_t* s = (wchar_t*)CoTaskMemAlloc((buffer.length() + 1) * sizeof(wchar_t));
        memcpy(s, buffer.data(), (buffer.length() + 1) * sizeof(wchar_t));
        *properties = s;
    }
    hr = S_OK;

cleanup:
    if (ppdl)
        ppdl->Release();
    if (ppd)
        ppd->Release();

    return hr;
}