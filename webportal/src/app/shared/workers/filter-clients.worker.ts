/// <reference lib="webworker" />

addEventListener('message', ({ data }) => {
    const { clients, searchValue } = data;
    const searchTerms = searchValue
        .toLowerCase()
        .normalize('NFD') // Normalize to separate letters and diacritics
        .replace(/\p{Diacritic}/gu, '') // Remove diacritics
        .split(' '); // Split into individual terms

    const filteredClients = clients
        .filter((client) => {
            // Convert both client_name and client_code to lowercase for comparison
            let clientName = '';
            if (client.primavera_client && client.primavera_client.nome) {
                clientName = client.primavera_client.nome
                    .toLowerCase()
                    .normalize('NFD')
                    .replace(/\p{Diacritic}/gu, ''); // Remove diacritics
            }

            const clientCode = (client.code ?? '').toLowerCase();

            // Check if every search term is found in either the client_name or client_code
            return searchTerms.every(
                // eslint-disable-next-line arrow-parens
                (term: any) =>
                    clientName.includes(term) || clientCode.includes(term)
            );
        })
        .slice(0, 20); // Limit results

    postMessage(filteredClients);
});
