﻿using iscaslune.Api.Application.Interfaces;
using iscaslune.Api.Domain.Context;
using iscaslune.Api.Domain.Entities;
using iscaslune.Api.Dtos.Categorias;
using iscaslune.Api.Infrastructure.Interfaces;
using iscaslune.Api.Infrastructure.Repositories;

namespace iscaslune.Api.Infrastructure.Cached;

public class CategoriaCached(IscasLuneContext context, ICachedService<Categoria> cachedService, CategoriaRepository categoriaRepository) 
    : GenericRepository<Categoria>(context), ICategoriaRepository
{
    private readonly CategoriaRepository _categoriaRepository = categoriaRepository;
    private readonly ICachedService<Categoria> _cachedService = cachedService;
    private const string _keyList = "categorias";

    public async Task<Categoria?> GetCategoriaByIdAsync(Guid id)
    {
        var key = id.ToString();
        var categoria = await _cachedService.GetItemAsync(key);

        if(categoria == null)
        {
            categoria = await _categoriaRepository.GetCategoriaByIdAsync(id);
            if(categoria != null)
            {
                categoria.Produtos.ForEach(x =>
                {
                    x.Categoria = null;
                });

                await _cachedService.SetItemAsync(key, categoria);
            }
        }

        return categoria;
    }

    public async Task<List<Categoria>?> GetCategoriasAsync(PaginacaoCategoriaDto paginacaoCategoriaDto)
    {
        if (!string.IsNullOrWhiteSpace(paginacaoCategoriaDto.Descricao)
            || paginacaoCategoriaDto.Asc)
            return await _categoriaRepository.GetCategoriasAsync(paginacaoCategoriaDto);

        var categorias = await _cachedService.GetListItemAsync(_keyList);

        if(categorias == null)
        {
            categorias = await _categoriaRepository.GetCategoriasAsync(paginacaoCategoriaDto);

            if(categorias != null && categorias.Count > 0)
                await _cachedService.SetListItemAsync(_keyList, categorias);
        }

        return categorias;
    }
}
