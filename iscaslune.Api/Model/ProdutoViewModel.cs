﻿using iscaslune.Api.Domain.Entities;
using iscaslune.Api.Model.Base;
using iscaslune.Api.Model.Categorias;
using iscaslune.Api.Model.Cores;
using iscaslune.Api.Model.Tamanhos;
using System.Text;
using System.Text.Json.Serialization;

namespace iscaslune.Api.Model;

public class ProdutoViewModel : BaseModel<Produto, ProdutoViewModel>
{
    public string Descricao { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EspecificacaoTecnica { get; set; }
    public string Foto { get; set; } = string.Empty;
    public List<TamanhoViewModel>? Tamanhos { get; set; } = new();
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<CorViewModel>? Cores { get; set; } = new();
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid CategoriaId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CategoriaViewModel? Categoria { get; set; } = null!;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Referencia { get; private set; }
    public override ProdutoViewModel? ForModel(Produto? entity)
    {
        if (entity == null) return null;

        Descricao = entity.Descricao;
        EspecificacaoTecnica = entity.EspecificacaoTecnica;
        Foto = Encoding.UTF8.GetString(entity.Foto);
        Tamanhos = entity.Tamanhos.Select(x => new TamanhoViewModel().ForModel(x) ?? new()).ToList();
        Cores = entity.Cores.Select(x => new CorViewModel().ForModel(x) ?? new()).ToList();
        Categoria = new CategoriaViewModel().ForModel(entity.Categoria);
        CategoriaId = entity.CategoriaId;
        Referencia = entity.Referencia;

        return this;
    }
}
