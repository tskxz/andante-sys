<p align="center">
  <img src="https://i.imgur.com/TqQrMYd.png" height="300px" width="300px"/>
</p>
<h1 align="center">AndanteSys</h1>  

# Sistema de Simulação Metro do Porto - Andante

Este projeto consiste num **Simulador do Ecossistema Intermodal Andante (Metro do Porto)** desenvolvido em C# com uma interface gráfica em **WPF (Windows Presentation Foundation)**. O objetivo principal é demonstrar a aplicação prática de conceitos avançados de **Programação Orientada a Objetos (POO)**

## Base de Funcionamento do Sistema

O programa baseia-se na simulação integrada de dois perfis de utilização que interagem sobre uma rede de transportes comumr:

### 1. Backoffice (Área do Administrador)
Permite a gestão global da infraestrutura e dos títulos de transporte. O administrador é responsável por alimentar o sistema, tendo a capacidade de emitir dois tipos de cartões com regras de negócio distintas:
* **Passe Andante Gold (Nominal):** Obrigatoriamente associado aos dados de uma `Pessoa` (Nome e NIF). Funciona por um sistema de assinaturas de zonas geográficas autorizadas (ex: `PRT1`).
* **Cartão Andante Azul (Ocasional/Anónimo):** Um título estritamente anónimo (sem dados pessoais associados), identificado por um ID gerado automaticamente e que funciona com base num saldo numérico de viagens.

### 2. Frontoffice (Área do Cliente / Simulador de Viagem)
Simula a interação física de um passageiro numa estação de metro real através de um **mecanismo de autenticação e validação**:
* **Autenticação Flexível (Login):** O ecrã do cliente inicia bloqueado. Para simular a aproximação de um cartão, o utilizador introduz o **NIF** (se possuir um passe Gold) ou o **ID do Cartão** (se possuir um cartão Azul).
* **Validação Inteligente (Polimorfismo):** Após escolher a estação de embarque, o utilizador aciona o validador virtual. O sistema utiliza polimorfismo para processar a leitura do cartão:
    * Para cartões **Gold**, o validador verifica se a zona da estação atual consta na lista de zonas permitidas do passe.
    * Para cartões **Azul**, o validador verifica se existe saldo disponível e, em caso afirmativo, debita automaticamente uma viagem ao cartão.
* **Feedback Visual:** A interface responde instantaneamente simulando a máquina física do Andante, exibindo uma **Luz Verde** (Acesso Autorizado) ou **Luz Vermelha** (Acesso Recusado por saldo insuficiente ou zona não autorizada).