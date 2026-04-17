using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaLegadoPedidos
{
    public class PedidoProcessor
    {
        private List<string> _logs = new List<string>();

        // MÉTODO PRINCIPAL: Responsável por processar toda a lógica de um pedido.
        // POSSUI ALTA COMPLEXIDADE (God Method) e muitas responsabilidades misturadas.
        public string ProcessarPedido(
            int pedidoId,
            string nomeCliente,
            string emailCliente,
            string tipoCliente,
            List<ItemPedido> itens,
            string cupom,
            string formaPagamento,
            string enderecoEntrega,
            double pesoTotal,
            bool entregaExpressa,
            bool clienteBloqueado,
            bool enviarEmail,
            bool salvarLog,
            string pais,
            int parcelas)
        {
            string resultado = "";
            double subtotal = 0;
            double desconto = 0;
            double frete = 0;
            double juros = 0;
            double total = 0;
            bool temErro = false;

            // --- ETAPA DE VALIDAÇÃO DE DADOS ---
            if (pedidoId <= 0)
            {
                resultado += "Pedido inválido\n";
                temErro = true;
            }

            if (string.IsNullOrEmpty(nomeCliente))
            {
                resultado += "Nome do cliente não informado\n";
                temErro = true;
            }

            // REGRA DE SEGURANÇA: Bloqueia o processamento se o cliente tiver restrições.
            if (clienteBloqueado == true)
            {
                resultado += "Cliente bloqueado\n";
                temErro = true;
            }

            // --- CÁLCULO DE ITENS E TAXAS ESPECÍFICAS ---
            if (itens == null || itens.Count == 0)
            {
                resultado += "Pedido sem itens\n";
                temErro = true;
            }
            else
            {
                for (int i = 0; i < itens.Count; i++)
                {
                    // Validação de integridade do item
                    if (itens[i].Quantidade <= 0 || itens[i].PrecoUnitario < 0)
                    {
                        resultado += "Item com dados inválidos: " + itens[i].Nome + "\n";
                        temErro = true;
                    }

                    subtotal = subtotal + (itens[i].PrecoUnitario * itens[i].Quantidade);

                    // REGRA DE NEGÓCIO: Taxas adicionais por categoria de produto (Alimentos e Importados)
                    if (itens[i].Categoria == "ALIMENTO")
                    {
                        subtotal = subtotal + 2;
                    }
