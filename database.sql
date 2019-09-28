-- phpMyAdmin SQL Dump
-- version 4.9.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Creato il: Set 28, 2019 alle 11:06
-- Versione del server: 10.4.6-MariaDB
-- Versione PHP: 7.3.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `habbo`
--

-- --------------------------------------------------------

--
-- Struttura della tabella `buddy`
--

CREATE TABLE `buddy` (
  `id` int(11) NOT NULL,
  `from` varchar(255) NOT NULL,
  `to` varchar(255) NOT NULL,
  `accept` int(11) NOT NULL DEFAULT 0
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `catalogue`
--

CREATE TABLE `catalogue` (
  `id` int(11) NOT NULL,
  `shortname` varchar(255) NOT NULL,
  `longname` varchar(255) NOT NULL,
  `itemname` varchar(255) NOT NULL,
  `itemdesc` varchar(255) NOT NULL,
  `price` int(11) NOT NULL DEFAULT 0,
  `rgb` varchar(255) NOT NULL DEFAULT '0,0,0',
  `size` varchar(255) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `catalogue`
--

INSERT INTO `catalogue` (`id`, `shortname`, `longname`, `itemname`, `itemdesc`, `price`, `rgb`, `size`) VALUES
(1, 'S1A', 'bed_armas_one', 'Single Bed', 'Rustic charm for one', 2, '0,0,0', '1 3'),
(28, 'KUMIANKKA', 'duck', 'Rubber Duck', 'Every bather needs one', 1, '0,0,0', '1 1'),
(29, 'TUN', 'chair_norja', 'Chair', 'Sleek and chico for each cheek', 2, '0,0,0', '1 1'),
(30, 'PEN', 'couch_norja', 'Bench', 'Two can perch comfortablt', 2, '0,0,0', '2 1'),
(31, 'PYN', 'table_norja_med', 'Coffee Table', 'Elegance embodied', 2, '0,0,0', '2 2'),
(32, 'HYN', 'shelves_norja', 'Bookcase', 'For nic naks and art deco books', 3, '0,0,0', '1 1'),
(33, 'PSN', 'soft_sofa_norja', 'Sofa', 'A soft iced sofa', 3, '0,0,0', '2 1'),
(35, 'RTV', 'red_tv', 'Portable TV', 'A portable TV for your habbo', 3, '0,0,0', '1 1'),
(34, 'BABA', 'bar_basic', 'Pura Minibar', 'A Pura series 300 minibar', 4, '0,0,0', '1 1');

-- --------------------------------------------------------

--
-- Struttura della tabella `chatloggs`
--

CREATE TABLE `chatloggs` (
  `id` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `text` text NOT NULL,
  `roomid` int(11) NOT NULL DEFAULT 0,
  `type` varchar(255) NOT NULL DEFAULT 'CHAT'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `favrooms`
--

CREATE TABLE `favrooms` (
  `id` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `roomid` int(11) NOT NULL DEFAULT 0
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `friendships`
--

CREATE TABLE `friendships` (
  `id` int(11) NOT NULL,
  `user1` int(11) NOT NULL,
  `user2` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struttura della tabella `groups_users`
--

CREATE TABLE `groups_users` (
  `id` int(11) NOT NULL,
  `group` int(11) NOT NULL,
  `user` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `permissions` int(11) NOT NULL,
  `time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struttura della tabella `heightmap`
--

CREATE TABLE `heightmap` (
  `id` int(11) NOT NULL,
  `model` varchar(255) NOT NULL DEFAULT 'model_a',
  `heightmap` text NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `heightmap`
--

INSERT INTO `heightmap` (`id`, `model`, `heightmap`) VALUES
(1, 'model_a', 'xxxxxxxxxxxx xxxx00000000 xxxx00000000 xxxx00000000 xxxx00000000 xxx000000000 xxxx00000000 xxxx00000000 xxxx00000000 xxxx00000000 xxxx00000000 xxxx00000000 xxxx00000000 xxxx00000000 xxxxxxxxxxxx xxxxxxxxxxxx'),
(2, 'model_e', 'xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xx0000000000 xx0000000000 x00000000000 xx0000000000 xx0000000000 xx0000000000 xx0000000000 xx0000000000 xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx'),
(3, 'lobby_a', 'XXXXXXXXX77777777777XXXXX XXXXXXXXX777777777777XXXX XXXXXXXXX777777777766XXXX XXXXXXXXX777777777755XXXX XX333333333333333334433XX XX333333333333333333333XX XX333333333333333333333XX 33333333333333333333333XX 333333XXXXXXX3333333333XX 333333XXXXXXX2222222222XX 333333XXXXXXX2222222222XX XX3333XXXXXXX2222222222XX XX3333XXXXXXX222222221111 XX3333XXXXXXX111111111111 333333XXXXXXX111111111111 3333333222211111111111111 3333333222211111111111111 3333333222211111111111111 XX33333222211111111111111 XX33333222211111111111111 XX3333322221111111XXXXXXX XXXXXXX22221111111XXXXXXX XXXXXXX22221111111XXXXXXX XXXXXXX22221111111XXXXXXX XXXXXXX22221111111XXXXXXX XXXXXXX222X1111111XXXXXXX XXXXXXX222X1111111XXXXXXX XXXXXXXXXXXX11XXXXXXXXXXX XXXXXXXXXXXX11XXXXXXXXXXX XXXXXXXXXXXX11XXXXXXXXXXX XXXXXXXXXXXX11XXXXXXXXXXX'),
(4, 'model_b', 'xxxxxxxxxxxx xxxxx0000000 xxxxx0000000 xxxxx0000000 xxxxx0000000 x00000000000 x00000000000 x00000000000 x00000000000 x00000000000 x00000000000 xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx'),
(5, 'model_c', 'xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx'),
(6, 'model_d', 'xxxxxxxxxxxx xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxx000000x xxxxxxxxxxxx'),
(7, 'model_f', 'xxxxxxxxxxxx xxxxxxx0000x xxxxxxx0000x xxx00000000x xxx00000000x xxx00000000x xxx00000000x x0000000000x x0000000000x x0000000000x x0000000000x xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx xxxxxxxxxxxx'),
(8, 'pool_a', 'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxx7xxxxxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxx777xxxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxx7777777xxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxx77777777xxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxx77777777xxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxx777777777xxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxx7xxx777777xxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxx7x777777777xxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxx7xxx77777777xxxxxxxxxxxxxxxx xxxxxxxxxxxxxxx7x777777777x7xxxxxxxxxxxxxxx xxxxxxxxxxxxxxx7xxx7777777777xxxxxxxxxxxxxx xxxxxxxxxxxxxxx777777777777xxxxxxxxxxxxxxxx xxxxxxxxxxxxxxx77777777777x2111xxxxxxxxxxxx xxxxxxxxxxxxxxx7777777777x221111xxxxxxxxxxx xxxxxxxxx7777777777777777x2211111xxxxxxxxxx xxxxxxxxx7777777777777777x22211111xxxxxxxxx xxxxxxxxx7777777777777777x222211111xxxxxxxx xxxxxx77777777777777777777x222211111xxxxxxx xxxxxx7777777xx777777777777x222211111xxxxxx xxxxxx7777777xx77777777777772222111111xxxxx xxxxxx777777777777777777777x22221111111xxxx xx7777777777777777777777x322222211111111xxx 77777777777777777777777x33222222111111111xx 7777777777777777777777x333222222211111111xx xx7777777777777777777x333322222222111111xxx xx7777777777777777777333332222222221111xxxx xx777xxx777777777777733333222222222211xxxxx xx777x7x77777777777773333322222222222xxxxxx xx777x7x7777777777777x33332222222222xxxxxxx xxx77x7x7777777777777xx333222222222xxxxxxxx xxxx77777777777777777xxx3222222222xxxxxxxxx xxxxx777777777777777777xx22222222xxxxxxxxxx xxxxxx777777777777777777x2222222xxxxxxxxxxx xxxxxxx777777777777777777222222xxxxxxxxxxxx xxxxxxxx7777777777777777722222xxxxxxxxxxxxx xxxxxxxxx77777777777777772222xxxxxxxxxxxxxx xxxxxxxxxx777777777777777222xxxxxxxxxxxxxxx xxxxxxxxxxx77777777777777x2xxxxxxxxxxxxxxxx xxxxxxxxxxxx77777777777777xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx777777777777xxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxx7777777777xxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxx77777777xxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxx777777xxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxx7777xxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxxx77xxxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx');

-- --------------------------------------------------------

--
-- Struttura della tabella `pubs`
--

CREATE TABLE `pubs` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `now_in` int(11) NOT NULL DEFAULT 0,
  `max_in` int(11) NOT NULL DEFAULT 25,
  `mapname` varchar(255) NOT NULL,
  `heightmap` varchar(255) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `pubs`
--

INSERT INTO `pubs` (`id`, `name`, `now_in`, `max_in`, `mapname`, `heightmap`) VALUES
(1, 'Main Lobby', 0, 25, 'lobby', 'lobby_a');

-- --------------------------------------------------------

--
-- Struttura della tabella `reports`
--

CREATE TABLE `reports` (
  `id` int(12) NOT NULL,
  `post` varchar(11) NOT NULL,
  `parent` int(11) NOT NULL,
  `type` int(11) NOT NULL,
  `by` int(11) NOT NULL,
  `state` int(11) NOT NULL,
  `time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `rights`
--

CREATE TABLE `rights` (
  `id` int(11) NOT NULL,
  `room` int(11) NOT NULL,
  `user` varchar(255) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `roomitems`
--

CREATE TABLE `roomitems` (
  `id` int(11) NOT NULL,
  `roomid` int(11) NOT NULL,
  `textname` varchar(255) NOT NULL,
  `itemname` varchar(255) NOT NULL,
  `itemdesc` varchar(255) NOT NULL,
  `rgb` varchar(255) NOT NULL DEFAULT '0,0,0',
  `size` varchar(255) NOT NULL DEFAULT '1 1',
  `shortname` varchar(255) NOT NULL,
  `rotate` int(11) NOT NULL DEFAULT 0,
  `x` int(11) NOT NULL DEFAULT 0,
  `y` int(11) NOT NULL DEFAULT 0
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `rooms`
--

CREATE TABLE `rooms` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `desc` varchar(255) NOT NULL,
  `owner` varchar(255) NOT NULL,
  `floor` varchar(255) NOT NULL DEFAULT 'floor1',
  `inroom` int(11) NOT NULL DEFAULT 0,
  `model` varchar(255) NOT NULL DEFAULT 'model_a',
  `door` varchar(255) NOT NULL DEFAULT 'open',
  `pass` varchar(255) NOT NULL,
  `space_w` int(11) NOT NULL DEFAULT 100,
  `space_f` int(11) NOT NULL DEFAULT 100
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `settings`
--

CREATE TABLE `settings` (
  `id` int(3) NOT NULL,
  `hotel_name` varchar(50) DEFAULT NULL,
  `max_users` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `settings`
--

INSERT INTO `settings` (`id`, `hotel_name`, `max_users`) VALUES
(1, 'Habbo Hotel', 5000);

-- --------------------------------------------------------

--
-- Struttura della tabella `useritems`
--

CREATE TABLE `useritems` (
  `id` int(11) NOT NULL,
  `user` varchar(255) NOT NULL,
  `textname` varchar(255) NOT NULL,
  `itemname` varchar(255) NOT NULL,
  `itemdesc` varchar(255) NOT NULL,
  `rgb` varchar(255) NOT NULL DEFAULT '0,0,0',
  `size` varchar(255) NOT NULL DEFAULT '1 1',
  `shortname` varchar(255) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `credits` int(255) NOT NULL DEFAULT 0,
  `email` varchar(255) NOT NULL,
  `figure` varchar(255) NOT NULL,
  `birthday` varchar(255) NOT NULL,
  `phonenumber` varchar(255) NOT NULL,
  `customData` varchar(255) NOT NULL,
  `had_read_agreement` int(11) NOT NULL DEFAULT 0,
  `sex` varchar(255) NOT NULL DEFAULT 'Male',
  `country` varchar(255) NOT NULL,
  `has_special_rights` varchar(255) NOT NULL DEFAULT '0',
  `badge_type` varchar(255) NOT NULL DEFAULT '0',
  `inroom` int(11) NOT NULL DEFAULT 0
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struttura della tabella `wordfilter`
--

CREATE TABLE `wordfilter` (
  `id` int(11) NOT NULL,
  `word` varchar(255) NOT NULL,
  `filter` varchar(255) NOT NULL DEFAULT 'bobba'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dump dei dati per la tabella `wordfilter`
--

INSERT INTO `wordfilter` (`id`, `word`, `filter`) VALUES
(10, 'cazzo', 'bobba');

--
-- Indici per le tabelle scaricate
--

--
-- Indici per le tabelle `buddy`
--
ALTER TABLE `buddy`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `catalogue`
--
ALTER TABLE `catalogue`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `chatloggs`
--
ALTER TABLE `chatloggs`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `favrooms`
--
ALTER TABLE `favrooms`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `friendships`
--
ALTER TABLE `friendships`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `groups_users`
--
ALTER TABLE `groups_users`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `heightmap`
--
ALTER TABLE `heightmap`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `pubs`
--
ALTER TABLE `pubs`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `reports`
--
ALTER TABLE `reports`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indici per le tabelle `rights`
--
ALTER TABLE `rights`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `roomitems`
--
ALTER TABLE `roomitems`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `rooms`
--
ALTER TABLE `rooms`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `settings`
--
ALTER TABLE `settings`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indici per le tabelle `useritems`
--
ALTER TABLE `useritems`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `wordfilter`
--
ALTER TABLE `wordfilter`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT per le tabelle scaricate
--

--
-- AUTO_INCREMENT per la tabella `buddy`
--
ALTER TABLE `buddy`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT per la tabella `catalogue`
--
ALTER TABLE `catalogue`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;

--
-- AUTO_INCREMENT per la tabella `chatloggs`
--
ALTER TABLE `chatloggs`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT per la tabella `favrooms`
--
ALTER TABLE `favrooms`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT per la tabella `friendships`
--
ALTER TABLE `friendships`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT per la tabella `groups_users`
--
ALTER TABLE `groups_users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT per la tabella `heightmap`
--
ALTER TABLE `heightmap`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT per la tabella `pubs`
--
ALTER TABLE `pubs`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT per la tabella `reports`
--
ALTER TABLE `reports`
  MODIFY `id` int(12) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT per la tabella `rights`
--
ALTER TABLE `rights`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT per la tabella `roomitems`
--
ALTER TABLE `roomitems`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=67;

--
-- AUTO_INCREMENT per la tabella `rooms`
--
ALTER TABLE `rooms`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT per la tabella `settings`
--
ALTER TABLE `settings`
  MODIFY `id` int(3) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT per la tabella `useritems`
--
ALTER TABLE `useritems`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT per la tabella `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT per la tabella `wordfilter`
--
ALTER TABLE `wordfilter`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
