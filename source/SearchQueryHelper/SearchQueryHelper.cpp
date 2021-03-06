// SearchQueryHelper.cpp : DLL アプリケーション用にエクスポートされる関数を定義します。
//

#include <searchapi.h>

#include <propkey.h>

extern "C" __declspec(dllexport)
HRESULT GetSearchQueryHelper(ISearchQueryHelper **ppSearchQueryHelper)
{
    *ppSearchQueryHelper = NULL;

    // Create an instance of the search manager

    ISearchManager *pSearchManager;
    HRESULT hr = CoCreateInstance(__uuidof(CSearchManager), NULL, CLSCTX_LOCAL_SERVER, IID_PPV_ARGS(&pSearchManager));
    if (FAILED(hr))
        return hr;

    // Get the catalog manager from the search manager
    ISearchCatalogManager *pSearchCatalogManager;
    hr = pSearchManager->GetCatalog(L"SystemIndex", &pSearchCatalogManager);
    if (FAILED(hr))
    {
        pSearchManager->Release();
        return hr;
    }

    // Get the query helper from the catalog manager
    hr = pSearchCatalogManager->GetQueryHelper(ppSearchQueryHelper);
    pSearchCatalogManager->Release();
    pSearchManager->Release();

    return hr;
}

extern "C" __declspec(dllexport)
ULONG ReleaseSearchQueryHelper(ISearchQueryHelper* pSearchQueryHelper)
{
    return pSearchQueryHelper->Release();
}

extern "C" __declspec(dllexport)
HRESULT GenerateSQLFromUserQuery(ISearchQueryHelper* pSearchQueryHelper, LPCWSTR pszQuery, LPWSTR* ppszSQL)
{
    return pSearchQueryHelper->GenerateSQLFromUserQuery(pszQuery, ppszSQL);
}

extern "C" __declspec(dllexport)
HRESULT get_ConnectionString(ISearchQueryHelper* pSearchQueryHelper, LPWSTR* pszConnectionString)
{
    return pSearchQueryHelper->get_ConnectionString(pszConnectionString);
}

extern "C" __declspec(dllexport)
HRESULT get_QueryContentLocale(ISearchQueryHelper* pSearchQueryHelper, LCID* pLcid)
{
    return pSearchQueryHelper->get_QueryContentLocale(pLcid);
}

extern "C" __declspec(dllexport)
HRESULT put_QueryContentLocale(ISearchQueryHelper* pSearchQueryHelper, LCID lcid)
{
    return pSearchQueryHelper->put_QueryContentLocale(lcid);
}

extern "C" __declspec(dllexport)
HRESULT get_QueryKeywordLocale(ISearchQueryHelper* pSearchQueryHelper, LCID* pLcid)
{
    return pSearchQueryHelper->get_QueryKeywordLocale(pLcid);
}

extern "C" __declspec(dllexport)
HRESULT put_QueryKeywordLocale(ISearchQueryHelper* pSearchQueryHelper, LCID lcid)
{
    return pSearchQueryHelper->put_QueryKeywordLocale(lcid);
}

extern "C" __declspec(dllexport)
HRESULT get_QueryContentProperties(ISearchQueryHelper* pSearchQueryHelper, LPWSTR* ppszContentProperties)
{
    return pSearchQueryHelper->get_QueryContentProperties(ppszContentProperties);
}

extern "C" __declspec(dllexport)
HRESULT put_QueryContentProperties(ISearchQueryHelper* pSearchQueryHelper, LPCWSTR pszContentProperties)
{
    return pSearchQueryHelper->put_QueryContentProperties(pszContentProperties);
}

extern "C" __declspec(dllexport)
HRESULT get_QueryMaxResults(ISearchQueryHelper* pSearchQueryHelper, LONG* pcMaxResults)
{
    return pSearchQueryHelper->get_QueryMaxResults(pcMaxResults);
}

extern "C" __declspec(dllexport)
HRESULT put_QueryMaxResults(ISearchQueryHelper* pSearchQueryHelper, LONG cMaxResults)
{
    return pSearchQueryHelper->put_QueryMaxResults(cMaxResults);
}

extern "C" __declspec(dllexport)
HRESULT get_QuerySelectColumns(ISearchQueryHelper* pSearchQueryHelper, LPWSTR* ppszSelectColumns)
{
    return pSearchQueryHelper->get_QuerySelectColumns(ppszSelectColumns);
}

extern "C" __declspec(dllexport)
HRESULT put_QuerySelectColumns(ISearchQueryHelper* pSearchQueryHelper, LPCWSTR pszSelectColumns)
{
    return pSearchQueryHelper->put_QuerySelectColumns(pszSelectColumns);
}

extern "C" __declspec(dllexport)
HRESULT get_QuerySorting(ISearchQueryHelper* pSearchQueryHelper, LPWSTR* ppszSorting)
{
    return pSearchQueryHelper->get_QuerySorting(ppszSorting);
}

extern "C" __declspec(dllexport)
HRESULT put_QuerySorting(ISearchQueryHelper* pSearchQueryHelper, LPCWSTR pszSorting)
{
    return pSearchQueryHelper->put_QuerySorting(pszSorting);
}

extern "C" __declspec(dllexport)
HRESULT get_QuerySyntax(ISearchQueryHelper* pSearchQueryHelper, SEARCH_QUERY_SYNTAX* pQuerySyntax)
{
    return pSearchQueryHelper->get_QuerySyntax(pQuerySyntax);
}

extern "C" __declspec(dllexport)
HRESULT put_QuerySyntax(ISearchQueryHelper* pSearchQueryHelper, SEARCH_QUERY_SYNTAX querySyntax)
{
    return pSearchQueryHelper->put_QuerySyntax(querySyntax);
}

extern "C" __declspec(dllexport)
HRESULT get_QueryTermExpansion(ISearchQueryHelper* pSearchQueryHelper, SEARCH_TERM_EXPANSION* pExpandTerms)
{
    return pSearchQueryHelper->get_QueryTermExpansion(pExpandTerms);
}

extern "C" __declspec(dllexport)
HRESULT put_QueryTermExpansion(ISearchQueryHelper* pSearchQueryHelper, SEARCH_TERM_EXPANSION expandTerms)
{
    return pSearchQueryHelper->put_QueryTermExpansion(expandTerms);
}

extern "C" __declspec(dllexport)
HRESULT get_QueryWhereRestrictions(ISearchQueryHelper* pSearchQueryHelper, LPWSTR* ppszRestrictions)
{
    return pSearchQueryHelper->get_QueryWhereRestrictions(ppszRestrictions);
}

extern "C" __declspec(dllexport)
HRESULT put_QueryWhereRestrictions(ISearchQueryHelper* pSearchQueryHelper, LPCWSTR pszRestrictions)
{
    return pSearchQueryHelper->put_QueryWhereRestrictions(pszRestrictions);
}
