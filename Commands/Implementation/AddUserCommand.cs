﻿using Commands.Abstraction;
using Core.Abstraction.Interfaces;
using Domain.Abstraction.Interfaces;
using Domain.Models;

namespace Commands.Implementation;

public class AddUserCommand : Command
{
    private readonly IUserService _userService;
    public AddUserCommand(IShowMessage showMessage,
        IUserService userService) : base(showMessage)
    {
        _userService = userService;
    }

    protected override string CommandString => "reg";

    protected override bool IsCommandFor(string input)
    {
        return input.Contains(CommandString);
    }

    protected override async Task<bool> InternalCommand()
    {
        long telegramId = 0;

        var resultParsing = long.TryParse(CommandArgs.ElementAt(0), out telegramId);

        if (CommandArgs.Count() != 1 || !resultParsing)
        {
            ShowMessage.ShowAsConsole("Not valid command");
            return await Task.FromResult(false);
        }
        
        var user = new User
        {
            TelegramId = telegramId
        };

        await _userService.AddUserAsync(user);


        ShowMessage.ShowAsConsole("Save user successful");


        return await Task.FromResult(true);
    }
}