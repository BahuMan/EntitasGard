using PlayerList.View;
using PlayerList.Model;
using System.Collections.Generic;

public class PlayerCtrl
{
    public List<IPlayerView> _view;
    private PlayerListModel _model;
    private PlayerViewFactory _factory;

    public PlayerCtrl(PlayerListModel model, PlayerViewFactory factory)
    {
        _model = model;
        _factory = factory;

        if (_model.Count == 0)
        {
            PlayerModel defaultPlayer = new PlayerModel() { Color = 0, Name = "Hooman", Team = 0, Type = PlayerTypeEnum.Human };
            _model.Add(defaultPlayer);
        }

        _view = new List<IPlayerView>(_model.Count);
        for (int p=0; p<_model.Count; ++p)
        {
            PlayerModel player = _model[p];
            IPlayerView pview = _factory.CreateNewPlayer();
            _view.Add(pview);
            Link(p, player, pview);
        }
    }

    private void Link(int nr, PlayerModel player, IPlayerView pview)
    {
        pview.SetColor(player.Color);
        pview.SetLineNr(nr);
        pview.SetName(player.Name);
        pview.SetPlayerType(player.Type);
        pview.SetTeam(player.Team);

        pview.OnAddLineClicked += pview_add;
        pview.OnRemoveLineClicked += pview_remove;
        pview.OnColorChosen += Pview_OnColorChosen;
        pview.OnNameEdited += Pview_OnNameEdited;
        pview.OnPlayertypeChosen += Pview_OnPlayertypeChosen;
        pview.OnTeamChosen += Pview_OnTeamChosen;

        player.OnColorChanged += (int color) => { pview.SetColor(color); };
        player.OnNameChanged += (string name) => { pview.SetName(name); };
        player.OnTeamChanged += (int team) => { pview.SetTeam(team); };
        player.OnTypeChanged += (PlayerTypeEnum pt) => { pview.SetPlayerType(pt); };
    }

    private void Pview_OnTeamChosen(int lineNr, int choiceNr)
    {
        if (_model[lineNr].Team != choiceNr) _model[lineNr].Team = choiceNr;
    }

    private void Pview_OnPlayertypeChosen(int lineNr, int choiceNr)
    {
        if (_model[lineNr].Type != (PlayerTypeEnum)choiceNr) _model[lineNr].Type = (PlayerTypeEnum) choiceNr;
    }

    private void Pview_OnNameEdited(int lineNr, string content)
    {
        if (!_model[lineNr].Name.Equals(content)) _model[lineNr].Name = content;
    }

    private void Pview_OnColorChosen(int lineNr, int choiceNr)
    {
        if  (_model[lineNr].Color != choiceNr) _model[lineNr].Color = choiceNr;
    }

    private void pview_remove(int lineNr)
    {
        if (_model.Count < 2) return; //don't remove last remaining line

        PlayerModel player = _model[lineNr];
        IPlayerView pview = _view[lineNr];
        pview.OnAddLineClicked -= pview_add;
        pview.OnRemoveLineClicked -= pview_remove;
        pview.OnColorChosen -= Pview_OnColorChosen;
        pview.OnNameEdited -= Pview_OnNameEdited;
        pview.OnPlayertypeChosen -= Pview_OnPlayertypeChosen;
        pview.OnTeamChosen -= Pview_OnTeamChosen;

        _model.RemoveAt(lineNr);
        _view.RemoveAt(lineNr);

        _factory.Destroy(pview);
        for (int p = lineNr; p < _view.Count; ++p)
        {
            _view[p].SetLineNr(p);
            _model[p].Color = p; //workaround, because I dont want to implement color picker
        }

    }

    private void pview_add(int lineNr)
    {
        //no more than 6 players
        if (_model.Count > 5) return;

        PlayerModel player = _model[lineNr].Duplicate();
        IPlayerView pview = _factory.CreateNewPlayer();
        Link(lineNr, player, pview);

        _model.Insert(lineNr, player);
        _view.Insert(lineNr, pview);

        for (int p=lineNr; p<_view.Count; ++p)
        {
            _view[p].SetLineNr(p);
            _model[p].Color = p; //workaround, because I dont want to implement color picker
        }
    }
}
