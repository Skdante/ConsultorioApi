using AutoMapper;
using ConsultorioApi.Core.Interfaces;
using ConsultorioApi.DataAccess;
using ConsultorioApi.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioApi.Core.Bussiness
{
    public class Catalogo : ICatalogo
    {
        private readonly ICatalogoRepositorio _catalogoRepositorio;
        private readonly IMapper _mapper;

        public Catalogo(ICatalogoRepositorio catalogoRepositorio,
            IMapper mapper)
        {
            _catalogoRepositorio = catalogoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<Estado>> GetEstados(int paisId)
        {
            List<Estado> estados = new List<Estado>();

            try
            {
                var estadosdb = await _catalogoRepositorio.GetEstado(paisId);
                estados = _mapper.Map<List<Estado>>(estadosdb);
            }
            catch (Exception ex)
            {
                Log.Error("Metodo GetEstados: {@paisId}", paisId, ex.Message);
            }
            return estados;
        }

        public async Task<List<Municipio>> GetMunicipios(int estadoId)
        {
            List<Municipio> municipios = new List<Municipio>();

            try
            {
                municipios = await _catalogoRepositorio.GetMunicipio(estadoId);
            }
            catch (Exception ex)
            {
                Log.Error("Metodo GetMunicipios: {@estadoId}", estadoId, ex.Message);
            }
            return municipios;
        }

        public async Task<List<Especialidad>> GetEspecialidades()
        {
            List<Especialidad> especialidades = new List<Especialidad>();

            try
            {
                especialidades = await _catalogoRepositorio.GetEspecialidad();
            }
            catch (Exception ex)
            {
                Log.Error("Metodo GetEspecialidades", ex.Message);
            }
            return especialidades;
        }
    }
}
