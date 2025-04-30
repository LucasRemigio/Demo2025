/// <reference lib="webworker" />

addEventListener('message', ({ data }) => {
    const { catalogProducts, searchValue } = data;
    const searchTerms = searchValue
        .toLowerCase()
        .normalize('NFD') // Normalize to separate letters and diacritics
        .replace(/\p{Diacritic}/gu, '') // Remove diacritics
        .split(' '); // Split into individual terms

    const filteredProducts = catalogProducts
        .filter((product) => {
            // Convert both description and product_code to lowercase for comparison
            const description = product.description
                .toLowerCase()
                .normalize('NFD')
                .replace(/\p{Diacritic}/gu, '');
            const productCode = (product.product_code ?? '').toLowerCase();

            // Check if every search term is found in either the description or product code
            return searchTerms.every(
                (term) =>
                    description.includes(term) || productCode.includes(term)
            );
        })
        .slice(0, 100); // Limit results

    postMessage(filteredProducts);
});
