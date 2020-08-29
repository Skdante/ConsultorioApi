using ConsultorioApi.Core;
using ConsultorioApi.Entities;
using ConsultorioApi.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            var userId = "";
            var mock = new Mock<ICompania>();
            var mockUser = new Mock<UserManager<ApplicationUser>>();
            var statusProcess = Task.Run(() => new StatusProcess() { Estatus = true });
            mock.Setup(x => x.CompaniaInsert(companiaInsert, userId)).Returns(statusProcess);
            var companiaController = new CompaniaController(mock.Object, mockUser.Object);

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
            var userId = "";
            var mock = new Mock<ICompania>();
            var mockUser = new Mock<UserManager<ApplicationUser>>();
            mock.Setup(x => x.CompaniaInsert(companiaInsert, userId));
            var companiaController = new CompaniaController(mock.Object, mockUser.Object);

            // Prueba
            var resultado = companiaController.Post(companiaInsert).Result;

            // Verificación
            Assert.AreEqual(expected: 500, actual: ((StatusCodeResult)resultado.Result).StatusCode);
        }
    }
}
