using System.Linq;
using Alura.ListaLeitura.Modelos;

namespace Alura.WebAPI.Api.Modelos
{
    public static class LivroFiltroExtension
    {
        public class LivroFiltro
        {
            public string Autor { get; set; }
            public string Titulo { get; set; }
            public string Subtitulo { get; set; }
            public string Lista { get; set; }
        }

        public static IQueryable<Livro> AplicaFiltro(this IQueryable<Livro> query, LivroFiltro filtro)
        {
            if (filtro != null)
            {
                if (!string.IsNullOrEmpty(filtro.Autor))
                    query = query.Where(a => a.Autor.Contains(filtro.Autor));

                if (!string.IsNullOrEmpty(filtro.Titulo))
                    query = query.Where(t => t.Titulo.Contains(filtro.Titulo));

                if (!string.IsNullOrEmpty(filtro.Subtitulo))
                    query = query.Where(s => s.Subtitulo.Contains(filtro.Subtitulo));

                if (!string.IsNullOrEmpty(filtro.Lista))
                    query = query.Where(l => l.Lista == filtro.Lista.ParaTipo());

            }

            return query;
        }
    }

}
