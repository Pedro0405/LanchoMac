using LanchoMac.Data;
using LanchoMac.Migrations;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace LanchoMac.Services.AutomaticMessages
{
    public class SendMessage : ISendMessage
    {
 
            private readonly LanchesContexto _lanchesContexto;

        public SendMessage(LanchesContexto lanchesContexto)
        {
            _lanchesContexto = lanchesContexto;
        }

        public void sendMessage(int id)
        {
            var Pix = "🌐";
            var Dinheiro = "💵";
            var Debito = "💵";
            var Credito = "💵";

            var initialMessage = "🎉SEU PEDIDO FOI CONFIRMADO🎉\n\n\n 🍴 Estamos preparando os ingredientes.\n\n Itens:\n";
            var EndMenssagem = "\n\n 🏍️Assim que sair para entrega, avisaremos.";
            var formaDepagamentoDef = "";
            var pedido = _lanchesContexto.Pedidos.FirstOrDefault(x => x.PedidoId == id);
            List<string> Lanches = new List<string>();
            if (pedido != null)
            {
                var formaDePagamento = pedido.FormaPagamento;
                var emojiFormaDepagamento = "";
                switch (formaDePagamento)
                {
                    case "Pix":
                        emojiFormaDepagamento = Pix;
                        break;
                    case "Dinheiro":
                        emojiFormaDepagamento = Dinheiro;
                        break;
                    case "Debito":
                        emojiFormaDepagamento = Debito;
                        break;
                    case "Credito":
                        emojiFormaDepagamento = Credito;
                        break;
                    // Adicione mais casos conforme necessário

                    // Caso padrão, se a forma de pagamento não corresponder a nenhum dos casos
                    default:
                        emojiFormaDepagamento = "❓";
                        break;
                }
                formaDePagamento = "\n\nForma de Pagamento:\n" + formaDePagamento + " " + emojiFormaDepagamento;
                formaDepagamentoDef = formaDePagamento;
                var PedidoDetalhe = pedido.PedidoItens;
                foreach (var item in PedidoDetalhe)
                {
                    Lanches.Add(item.Quantidade + "-" + item.Lanche.Nome);
                }
                if (Lanches.Any())
                {
                    foreach (var item in Lanches)
                    {
                        initialMessage += $"{item}\n";
                    }
                }
            }

            var accountSid = "AC89589b61ba1177df80573f8d4be30160";
            var authToken = "cd0c531e58f116d3ab072182ef592b9e";
            TwilioClient.Init(accountSid, authToken);
            var menssagem = initialMessage + formaDepagamentoDef + EndMenssagem;

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber("whatsapp:+5521981685533"));
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = menssagem;

            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);

        }
    }
}
