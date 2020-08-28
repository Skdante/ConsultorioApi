using ConsultorioApi.Core;
using ConsultorioApi.Entities;
using ConsultorioApi.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApi.Test.Controlador
{
    [TestClass]
    public class CompaniaControllerTest
    {
        [TestMethod]
        public void Post_ConfirmarCompania()
        {
            // Preparación
            var companiaInsert = new CompaniaInsert();
            var mock = new Mock<ICompania>();
            var statusProcess = Task.Run(() => new StatusProcess() { Estatus = true });
            mock.Setup(x => x.CompaniaInsert(companiaInsert)).Returns(statusProcess);
            var companiaController = new CompaniaController(mock.Object);

            // Prueba
            var resultado = companiaController.Post(companiaInsert).Result;

            // Verificación
            Assert.AreEqual(expected: 200, actual: ((ObjectResult)resultado.Result).StatusCode);
            Assert.AreEqual(expected: true, actual: ((StatusProcess)((ObjectResult)resultado.Result).Value).Estatus);
        }

        [TestMethod]
        public void Post_ConfirmarCompania_Error()
        {
            // Preparación
            var companiaInsert = new CompaniaInsert();
            var mock = new Mock<ICompania>();
            mock.Setup(x => x.CompaniaInsert(companiaInsert));
            var companiaController = new CompaniaController(mock.Object);

            // Prueba
            var resultado = companiaController.Post(companiaInsert).Result;

            // Verificación
            Assert.AreEqual(expected: 500, actual: ((StatusCodeResult)resultado.Result).StatusCode);
        }
    }
}
