using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ouvidoria_social.Models.Utils;
using Ouvidoria_Social_DataAccess;

namespace ouvidoria_social.Models.Adapter
{
    public class UsuarioAdapter
    {
        [Required(ErrorMessage = "É necessário informar o usuário.")]
        [Remote("VerificaUsuario", "Autenticacao", ErrorMessage = "Este Logind de usuário já exite.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "É necessário informar a senha.")]
        public string Senha { get; set; }
        
        [System.ComponentModel.DataAnnotations.Compare("Senha", ErrorMessage = "A senha e a confirmação da senha são diferentes.")]
        public string ConfirmaPassword { get; set; }

        [Required(ErrorMessage = "É necessário informar a sua senha antiga.")]
        [Remote("VerificaSenhaAntiga", "Autenticacao", ErrorMessage = "Sua senha antiga não corresponde a senha do usuário ativo.")]
        public string OldPassword { get; set; }

        public Usuario ConvertToUsuario()
        {
            return new Usuario()
            {
                   Login = this.Login,
                   Senha = Criptografia.Codifica(this.Senha)
            };
        }
    }
}